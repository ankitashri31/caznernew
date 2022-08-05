using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.AzureBlobStorage;
using CaznerMarketplaceBackendApp.BannerLogo;
using CaznerMarketplaceBackendApp.MultiTenancy;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.Product.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static CaznerMarketplaceBackendApp.AppConsts;

namespace CaznerMarketplaceBackendApp.Web.Controllers
{
     [AbpAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductFileController : CaznerMarketplaceBackendAppControllerBase
    {
        private readonly IRepository<ProductImages, long> _productImagesRepository;
        private readonly IRepository<ProductMediaImages, long> _productMediaImagesRepository;
        private readonly IRepository<ProductBrandingPosition, long> _productBrandingPositionRepository;
        private readonly IRepository<UserBannerLogoData, long> _userBannerLogoDataRepository;
        private IConfiguration _configuration;
        string FolderName = string.Empty;
        string LogoFolderName = string.Empty;
        string CurrentWebsiteUrl = string.Empty;
        string AzureProductFolder = string.Empty;
        string AzureStorageUrl = string.Empty;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IAzureBlobStorageService _azureBlobStorageService;
        private readonly IRepository<Tenant, int> _tenantManager;
        public ProductFileController(IRepository<ProductImages, long> productImagesRepository, IRepository<ProductMediaImages, long> productMediaImagesRepository, IRepository<ProductBrandingPosition, long> productBrandingPositionRepository, 
            IConfiguration configuration, IRepository<UserBannerLogoData, long> userBannerLogoDataRepository, IHttpContextAccessor httpContextAccessor, IAzureBlobStorageService azureBlobStorageService, IRepository<Tenant, int> tenantManager)
        {
            _productImagesRepository = productImagesRepository;
            _productMediaImagesRepository = productMediaImagesRepository;
            _productBrandingPositionRepository = productBrandingPositionRepository;
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
            _configuration = configuration;
            FolderName = _configuration["FileUpload:FolderName"];
            LogoFolderName = _configuration["FileUpload:FrontEndLogoFolderName"];
            CurrentWebsiteUrl = _configuration["FileUpload:WebsireDomainPath"];
            AzureProductFolder = _configuration["FileUpload:AzureProductFolder"];
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];
            _userBannerLogoDataRepository = userBannerLogoDataRepository;
            _httpContextAccessor = httpContextAccessor;
            _azureBlobStorageService = azureBlobStorageService;
            _tenantManager = tenantManager;
        }

        [HttpPost]
        [Route("UploadProductImageFile")]
        public async Task<ProductImagesDto> UploadProductImageFile()
        {
            ProductImagesDto Response = new ProductImagesDto();
            Guid ImageName = Guid.NewGuid();
            bool IsInvalidType = false;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath= $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                var files = HttpContext.Request.Form.Files;
                //var files = file;
                ProductImages Model = new ProductImages();
                foreach (var images in files)
                {

                    using (var memoryStream = new MemoryStream())
                    {
                        await images.CopyToAsync(memoryStream);
                        Model.ImageFileData = memoryStream.ToArray();
                    }
                    var ImageExt = images.ContentType.Split('/');
                    string FilePath = await _azureBlobStorageService.SaveBlobImage(Model.ImageFileData, folderPath, ImageName.ToString() + "." + ImageExt[1], images.ContentType);
                    if (ImageExtensions.Contains(ImageExt[1].ToUpperInvariant()))
                    {
                        Model.ImageName = ImageName.ToString() + "." + ImageExt[1];
                        Model.TenantId = 1;//AbpSession.TenantId.Value;
                        Model.ImagePath = FilePath;
                        Model.IsProductSubmissionDone = false;
                        long Id = await _productImagesRepository.InsertAndGetIdAsync(Model);
                        Response.Id = Id;
                        Response.ImageName = ImageName.ToString() + "." + ImageExt[1];
                        Response.ImagePath = FilePath;
                        Response.MediaType = 0;
                    }
                    else
                    {
                        IsInvalidType = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Msg = "File could not be uploaded";
            }
            if (!IsInvalidType)
            {
                return Response;
            }
            else
            {
                throw new UserFriendlyException(L("InvalidImageFileType"));
            }
        }

        public static readonly List<string> ImageExtensions = new List<string> { "JPG", "JPEG", "JPE", "BMP", "PNG" };
        public static readonly List<string> PdfExtensions = new List<string> { "PDF", "JPEG", "PNG", "OCTET-STREAM" };
        [HttpPost]
        [Route("UploadProductMediaLineArt")]
        public async Task<ProductMediaImageDto> UploadProductMediaLineArt()
        {

            string PhysicalFilePath = _configuration["FileUpload:PhysicalFileLocation"];
            string test = _configuration.GetValue<string>("FileUpload:PhysicalFileLocation");
            ProductMediaImageDto Response = new ProductMediaImageDto();
            Guid ImageName = Guid.NewGuid(); 
            var files = HttpContext.Request.Form.Files;
            bool IsInvalidType = false;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                foreach (var images in files)
                {
                    ProductMediaImages Model = new ProductMediaImages();
                    using (var memoryStream = new MemoryStream())
                    {
                        await images.CopyToAsync(memoryStream);
                        Model.ImageFileData = memoryStream.ToArray();
                    }
                    var ImageExt = images.ContentType.Split('/');

                    if (PdfExtensions.Contains(ImageExt[1].ToUpperInvariant()))
                    {
                        if (ImageExt[1].ToUpperInvariant() == "OCTET-STREAM")
                            ImageExt[1] = "PSD";
                        string FilePath = await _azureBlobStorageService.SaveBlobImage(Model.ImageFileData, folderPath, ImageName.ToString() + "." + ImageExt[1], images.ContentType);
                        Response.FileName = ImageName.ToString() + "." + ImageExt[1];
                        Response.MediaType = 1;//(int)ProductMediaTypes.LineArt;

                    }
                    else
                    {
                        IsInvalidType = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (!IsInvalidType)
            { return Response; }
            else
            {
                throw new UserFriendlyException(L("InvalidImageFileType"));
            }
        }

        [HttpPost]
        [Route("UploadProductMediaLifeStyleImages")]
        public async Task<ProductMediaImageDto> UploadProductMediaLifeStyleImages()
        {
            ProductMediaImageDto Response = new ProductMediaImageDto();
            Guid ImageName = Guid.NewGuid();
            var files = HttpContext.Request.Form.Files;
            bool IsInvalidType = false;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                foreach (var images in files)
                {
                    ProductMediaImages Model = new ProductMediaImages();
                    using (var memoryStream = new MemoryStream())
                    {
                        await images.CopyToAsync(memoryStream);
                        Model.ImageFileData = memoryStream.ToArray();
                    }
                    var ImageExt = images.ContentType.Split('/');

                    if (ImageExtensions.Contains(ImageExt[1].ToUpperInvariant()))
                    {
                        string FilePath = await _azureBlobStorageService.SaveBlobImage(Model.ImageFileData, folderPath, ImageName.ToString() + "." + ImageExt[1], images.ContentType);
                        Response.FileName = ImageName.ToString() + "." + ImageExt[1];
                        Response.MediaType = 2;//(int)ProductMediaTypes.LifeStyleImages;
                    }
                    else
                    {
                        IsInvalidType = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (!IsInvalidType)
            { return Response; }
            else
            {
                throw new UserFriendlyException(L("InvalidImageFileType"));
            }
        }

        [HttpPost]
        [Route("UploadProductMediaOtherMedia")]
        public async Task<ProductMediaImageDto> UploadProductMediaOtherMedia()
        {
            ProductMediaImageDto Response = new ProductMediaImageDto();
            Guid ImageName = Guid.NewGuid();
            var files = HttpContext.Request.Form.Files;
            bool IsInvalidType = false;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                foreach (var images in files)
                {
                    ProductMediaImages Model = new ProductMediaImages();
                    using (var memoryStream = new MemoryStream())
                    {
                        await images.CopyToAsync(memoryStream);
                        Model.ImageFileData = memoryStream.ToArray();
                    }
                    var ImageExt = images.ContentType.Split('/');

                    if (ImageExtensions.Contains(ImageExt[1].ToUpperInvariant()))
                    {
                        string FilePath = await _azureBlobStorageService.SaveBlobImage(Model.ImageFileData, folderPath, ImageName.ToString() + "." + ImageExt[1], images.ContentType);
                        Response.FileName = ImageName.ToString() + "." + ImageExt[1];
                        Response.MediaType = 5;//(int)ProductMediaTypes.OtherMedia;
                    }
                    else
                    {
                        IsInvalidType = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (!IsInvalidType)
            { return Response; }
            else
            {
                throw new UserFriendlyException(L("InvalidImageFileType"));
            }
        }

        [HttpPost]
        [Route("UploadBrandingPositionImages")]
        public async Task<List<ProductBrandingPositionDto>> UploadBrandingPositionImages()
        {
            List<ProductBrandingPositionDto> Response = new List<ProductBrandingPositionDto>();
            bool IsInvalidType = false;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                var files = HttpContext.Request.Form.Files;
              
                foreach (var images in files)
                {
                    Guid ImageName = Guid.NewGuid();
                    IsInvalidType = false;                                                
                    byte[] FileData;
                    using (var memoryStream = new MemoryStream())
                    {
                        await images.CopyToAsync(memoryStream);
                        FileData = memoryStream.ToArray();
                    }
                    var ImageExt = images.ContentType.Split('/');
                    if (ImageExtensions.Contains(ImageExt[1].ToUpperInvariant()))
                    {
                        string FilePath = await _azureBlobStorageService.SaveBlobImage(FileData, folderPath, ImageName.ToString() + "." + ImageExt[1], images.ContentType);
                        Response.Add(new ProductBrandingPositionDto() 
                        {
                          ImageName = ImageName.ToString() + "." + ImageExt[1], 
                          ImageFileURL = FilePath,
                          LayerTitle = "",
                          Extension = ImageExt[1]
                        }); 
                    }
                    else
                    {
                        IsInvalidType = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (!IsInvalidType)
            { return Response; }
            else
            {
                throw new UserFriendlyException(L("InvalidImageFileType"));
            }
        }

        [HttpPost]
        [Route("FileUploadToPhysicalLocation")]
        public async Task<ResponseModel> FileUploadToPhysicalLocation()
        {
            ResponseModel Response = new ResponseModel();
            bool IsInvalidType = false;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                var files = HttpContext.Request.Form.Files;
                byte[] ImageArray;
                foreach (var images in files)
                {
                    Guid ImageName = Guid.NewGuid();
                    IsInvalidType = false;
                
                    using (var memoryStream = new MemoryStream())
                    {
                        await images.CopyToAsync(memoryStream);
                        ImageArray = memoryStream.ToArray();
                    }
                    var ImageExt = images.ContentType.Split('/');
                    if (ImageExtensions.Contains(ImageExt[1].ToUpperInvariant()))
                    {
                        string FilePath = await _azureBlobStorageService.SaveBlobImage(ImageArray, folderPath, ImageName.ToString() + "." + ImageExt[1], images.ContentType);
                        Response.FileName = ImageName.ToString() + "." + ImageExt[1];
                        Response.MediaType = 0;
                    }
                    else
                    {
                        IsInvalidType = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (!IsInvalidType)
            { return Response; }
            else
            {
                throw new UserFriendlyException(L("InvalidImageFileType"));
            }
        }


        [HttpPost]
        [Route("FileUploadToPhysicalLocationList")]
        public async Task<List<ResponseModel>> FileUploadToPhysicalLocationList()
        {
            List<ResponseModel> Response =  new List<ResponseModel>();
            bool IsInvalidType = false;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                var files = HttpContext.Request.Form.Files;
                byte[] ImageArray;
                foreach (var images in files)
                {
                    ResponseModel resp = new ResponseModel();
                    Guid ImageName = Guid.NewGuid();
                    IsInvalidType = false;

                    using (var memoryStream = new MemoryStream())
                    {
                        await images.CopyToAsync(memoryStream);
                        ImageArray = memoryStream.ToArray();
                    }
                    var ImageExt = images.ContentType.Split('/');
                    if (ImageExtensions.Contains(ImageExt[1].ToUpperInvariant()))
                    {
                        string FilePath = await _azureBlobStorageService.SaveBlobImage(ImageArray, folderPath, ImageName.ToString() + "." + ImageExt[1], images.ContentType);
                        resp.FileName = ImageName.ToString() + "." + ImageExt[1];
                        resp.MediaType = 0;
                        Response.Add(resp);
                    }
                    else
                    {
                        IsInvalidType = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (!IsInvalidType)
            { return Response; }
            else
            {
                throw new UserFriendlyException(L("InvalidImageFileType"));
            }
        }


        [HttpPost]
        [Route("UploadLogoBannerImageFile")]
        public async Task<List<ResponseModel>> UploadLogoBannerImageFile(int ImageType, int? PageTypeId)
        {
           
            bool IsInvalidType = false;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            List<ResponseModel> ResponseList = new List<ResponseModel>();
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                var files = HttpContext.Request.Form.Files;
                string Output = string.Empty;

                byte[] ImageArray;
                foreach (var images in files)
                {
                    ResponseModel resp = new ResponseModel();
                    UserBannerLogoData userlogo = new UserBannerLogoData();
                    Guid ImageName = Guid.NewGuid();
                    IsInvalidType = false;

                    using (var memoryStream = new MemoryStream())
                    {
                        await images.CopyToAsync(memoryStream);
                        ImageArray = memoryStream.ToArray();
                    }
                    var ImageExt = images.ContentType.Split('/');
                    if (ImageExtensions.Contains(ImageExt[1].ToUpperInvariant()))
                    {
                        string FilePath = await _azureBlobStorageService.SaveBlobImage(ImageArray, folderPath, ImageName.ToString() + "." + ImageExt[1], images.ContentType);
                        userlogo.ImageData = ImageArray;
                        userlogo.ImageName = ImageName.ToString();
                        userlogo.ImageTypeId = Convert.ToInt64(ImageType);
                        userlogo.ImageUrl = FilePath;
                        if (PageTypeId.HasValue)
                        {
                            userlogo.PageTypeId = Convert.ToInt64(PageTypeId);
                        }
                     
                        userlogo.Ext = ImageExt[1].ToLowerInvariant();
                        userlogo.Url = FilePath;
                        userlogo.Name = ImageName.ToString();
                        userlogo.Size = images.Length.ToString();
                        userlogo.Type = images.ContentType; 
                        userlogo.IsActive = true;
                        userlogo.TenantId = TenantId;
                        userlogo.UserId = AbpSession.UserId.Value;
                        long Id = await _userBannerLogoDataRepository.InsertAndGetIdAsync(userlogo);

                        //--------------bind response list-----------------------
                        resp.Id = Id;
                        resp.FileName = ImageName.ToString() + "." + ImageExt[1];
                        resp.MediaType = ImageType;
                        resp.Ext = ImageExt[1].ToLowerInvariant();
                        resp.Url = FilePath;
                        resp.Name = ImageName.ToString();
                        resp.Size = images.Length.ToString();
                        resp.Type = images.ContentType;
                        ResponseList.Add(resp);
                    }
                    else
                    {
                        IsInvalidType = true;
                    }
                }
                if (!IsInvalidType)
                { return ResponseList; }
                else
                {
                    throw new UserFriendlyException(L("InvalidImageFileType"));
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.StackTrace);
            }
        }

        [HttpPost]
        [Route("UploadBannerImageFile")]
        public async Task<ResponseModel> UploadBannerImageFile()
        {
            ResponseModel resp = new ResponseModel();
            bool IsInvalidType = false;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                var files = HttpContext.Request.Form.Files;
                byte[] ImageArray;
                foreach (var images in files)
                {
                    UserBannerLogoData userlogo = new UserBannerLogoData();
                    Guid ImageName = Guid.NewGuid();
                    IsInvalidType = false;

                    using (var memoryStream = new MemoryStream())
                    {
                        await images.CopyToAsync(memoryStream);
                        ImageArray = memoryStream.ToArray();
                    }
                    var ImageExt = images.ContentType.Split('/');
                    if (ImageExtensions.Contains(ImageExt[1].ToUpperInvariant()))
                    {
                        string FilePath = await _azureBlobStorageService.SaveBlobImage(ImageArray, folderPath, ImageName.ToString() + "." + ImageExt[1], images.ContentType);
                        userlogo.ImageData = ImageArray;
                        userlogo.ImageName = ImageName.ToString();
                        userlogo.ImageTypeId = 2;
                        userlogo.ImageUrl = FilePath;
                        userlogo.IsActive = true;
                        userlogo.TenantId = TenantId;
                        userlogo.UserId = AbpSession.UserId.Value;
                        await _userBannerLogoDataRepository.InsertAsync(userlogo);
                        resp.FileName = ImageName.ToString() + "." + ImageExt[1];
                        resp.MediaType = 0;
                    }
                    else
                    {
                        IsInvalidType = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (!IsInvalidType)
            { return resp; }
            else
            {
                throw new UserFriendlyException(L("InvalidImageFileType"));
            }
        }

        #region Save images using gallery urls

        [HttpPost]
        [Route("SaveImagesByCommaSepUrls")]
        public async Task<List<ProductImageType>> SaveImagesByCommaSepUrls(ImagesRequest Input)
        {
            List<ProductImageType> ImageList = new List<ProductImageType>();
            try
            {
                string TenantId = string.Empty;
                WebClient myWebClient = new WebClient();
                Utility utility = new Utility();

                if (string.IsNullOrEmpty(Input.EncryptedTenantId))
                {
                    int? SessionTenantId = AbpSession.TenantId;
                    if (SessionTenantId.HasValue)
                    {
                        TenantId = SessionTenantId.ToString();
                    }
                }
                else
                {
                    TenantId = utility.DecryptString(Input.EncryptedTenantId);
                }
                if (string.IsNullOrEmpty(TenantId))
                {
                    throw new AbpAuthorizationException("Tenant user is unauthorized");
                }

                var Tenant = await _tenantManager.FirstOrDefaultAsync(Convert.ToInt32(TenantId));

                foreach (var image in Input.UrlList)
                {
                    ProductImageType Image = new ProductImageType();
                    string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
                    Guid ImageName = Guid.NewGuid();

                    var request = WebRequest.CreateHttp(image);
                    //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.1";
                    var response = (HttpWebResponse)(await request.GetResponseAsync());
                    var length = response.ContentLength;

                    byte[] bytes = myWebClient.DownloadData(image);
                    string ext = Path.GetExtension(image);
                    string type = "image/" + ext;
                    string Length = length.ToString();

                    string FilePath = await _azureBlobStorageService.SaveBlobImage(bytes, folderPath, ImageName.ToString() + "." + ext, response.ContentType);
                    Image.Ext = ext;
                    Image.Size = length.ToString();
                    Image.Type = "image/" + ext;
                    Image.Name = ImageName.ToString();
                    Image.Url = FilePath;
                    Image.ImagePath = FilePath;
                    ImageList.Add(Image);
                }
            }
            catch (Exception ex)
            {

            }
            return ImageList;
        }
        #endregion

        [HttpPost]
        [Produces("application/json")]
        [Route("UpdateBannerLogoData")]
        public async Task<List<UserLogoBannerDataDto>> UpdateBannerLogoData(UserLogoBannerResultRequestDto Input)
        {
            List<UserLogoBannerDataDto> BannerList = new List<UserLogoBannerDataDto>();
            Utility utility = new Utility();
            string TenantId = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(Input.EncryptedTenantId))
                {
                    int? SessionTenantId = AbpSession.TenantId;
                    if (SessionTenantId.HasValue)
                    {
                        TenantId = SessionTenantId.ToString();
                    }
                }
                else
                {
                    TenantId = utility.DecryptString(Input.EncryptedTenantId);
                }
                if (string.IsNullOrEmpty(TenantId))
                {
                    throw new AbpAuthorizationException("Tenant user is unauthorized");
                }
              
                List<ResponseModel> ResponseList = new List<ResponseModel>();
                //user banner logo images update  
                if (Input != null)
                {
                    #region Update
                    var OldData = Input.InputData.Where(i => i.Id > 0);
                    foreach (var image in OldData)
                    {
                        ResponseModel resp = new ResponseModel();
                        var isExists = _userBannerLogoDataRepository.GetAll().Where(i => i.Id == image.Id).FirstOrDefault();
                        if (isExists != null)
                        {
                            isExists.ImageName = image.FileName;
                            isExists.Name = image.FileName;
                            isExists.ImageUrl = image.ImageUrl;
                            isExists.Ext = image.Ext;
                            isExists.Size = image.Size;
                            isExists.Url = image.Url;
                            await _userBannerLogoDataRepository.UpdateAsync(isExists);


                            //--------------bind response list-----------------------
                            resp.Id = isExists.Id;
                            resp.FileName = image.FileName + "." + image.Ext;
                            resp.MediaType = Input.ImageType;
                            resp.Ext = image.Ext.ToLowerInvariant();
                            resp.Url = image.ImageUrl;
                            resp.Name = image.FileName.ToString();
                            resp.Size = image.Size.ToString();
                            resp.Type = image.Type;
                            ResponseList.Add(resp);
                        }
                    }
                    #endregion

                    #region Add New
                    //var Tenant = await _tenantManager.FirstOrDefaultAsync(AbpSession.TenantId.Value);
                    //string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";

                    //var files = HttpContext.Request.Form.Files;
                    //byte[] ImageArray;
                    //foreach (var images in files)
                    //{
                    //    ResponseModel resp = new ResponseModel();
                    //    UserBannerLogoData userlogo = new UserBannerLogoData();
                    //    Guid ImageName = Guid.NewGuid();

                    //    using (var memoryStream = new MemoryStream())
                    //    {
                    //        await images.CopyToAsync(memoryStream);
                    //        ImageArray = memoryStream.ToArray();
                    //    }
                    //    var ImageExt = images.ContentType.Split('/');
                    //    if (ImageExtensions.Contains(ImageExt[1].ToUpperInvariant()))
                    //    {
                    //        string FilePath = await _azureBlobStorageService.SaveBlobImage(ImageArray, folderPath, ImageName.ToString() + "." + ImageExt[1], images.ContentType);
                    //        userlogo.ImageData = ImageArray;
                    //        userlogo.ImageName = ImageName.ToString();
                    //        userlogo.ImageTypeId = Convert.ToInt64(Input.ImageType);
                    //        userlogo.ImageUrl = FilePath;
                    //        if (Input.PageTypeId.HasValue)
                    //        {
                    //            userlogo.PageTypeId = Convert.ToInt64(Input.PageTypeId);
                    //        }

                    //        userlogo.Ext = ImageExt[1].ToLowerInvariant();
                    //        userlogo.Url = FilePath;
                    //        userlogo.Name = ImageName.ToString();
                    //        userlogo.Size = images.Length.ToString();
                    //        userlogo.Type = images.ContentType;
                    //        userlogo.IsActive = true;
                    //        userlogo.TenantId = AbpSession.TenantId.Value;
                    //        userlogo.UserId = AbpSession.UserId.Value;
                    //        long Id = await _userBannerLogoDataRepository.InsertAndGetIdAsync(userlogo);

                    //        //--------------bind response list-----------------------
                    //        resp.Id = Id;
                    //        resp.FileName = ImageName.ToString() + "." + ImageExt[1];
                    //        resp.MediaType = Input.ImageType;
                    //        resp.Ext = ImageExt[1].ToLowerInvariant();
                    //        resp.Url = FilePath;
                    //        resp.Name = ImageName.ToString();
                    //        resp.Size = images.Length.ToString();
                    //        resp.Type = images.ContentType;
                    //        ResponseList.Add(resp);
                    //    }
                    //}
                    #endregion
                }
                return BannerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class ImagesRequest
        {
            public string EncryptedTenantId { get; set; }
            public string[] UrlList { get; set; }
        }


        public class ResponseModel
        {
            public long Id { get; set; }
            public string FileName { get; set; }
            public int MediaType { get; set; }
            public string Ext { get; set; }
            public string Size { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
        }

        #region Save file in www root location of solution
        private string SaveFileInWWWRootLocation(byte[] ImgStr, string ImgName, string WebrootPath)
        {
            string FileLocation = string.Empty;
            try
            {
                //Check if directory exist
                if (!System.IO.Directory.Exists(WebrootPath))
                {
                    System.IO.Directory.CreateDirectory(WebrootPath); //Create directory if it doesn't exist
                }

                string imageName = ImgName;

                //set the image path
                string imgPath = Path.Combine(WebrootPath, imageName);

                byte[] imageBytes = ImgStr;

                System.IO.File.WriteAllBytes(imgPath, imageBytes);
                FileLocation = WebrootPath + "//" + imageName;
            }
            catch (Exception ex)
            {


            }
            return FileLocation;
        }
        #endregion

    }
}
