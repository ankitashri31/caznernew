using System;
using System.Collections.Generic;
using CaznerMarketplaceBackendApp.Product.Dto;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Collections.Extensions;
using System.Linq;
using Abp.Authorization;
using Abp.Domain.Uow;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.BrandingMethod.Dto;
using CaznerMarketplaceBackendApp.Product.Masters;
using CaznerMarketplaceBackendApp.BannerLogo;
using FimApp.EntityFrameworkCore;
using CaznerMarketplaceBackendApp.BrandingMethods;
using CaznerMarketplaceBackendApp.Connections;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using NUglify.Helpers;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text;
using System.Net;
using Abp.Threading;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductFrontEndAppService : CaznerMarketplaceBackendAppAppServiceBase, IProductFrontEndAppService
    {
        private readonly IRepository<ProductMaster, long> _repository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<UserBannerLogoData, long> _userBannerLogoDataRepository;
        private readonly IRepository<CategoryMaster, long> _categoryMasterRepository;
        private readonly IRepository<BrandingMethodMaster, long> _repositoryBrandingMethod;
        private readonly IRepository<ProductImages, long> _productImagesRepository;
        private readonly IRepository<ProductMediaImages, long> _productMediaImagesRepository;
        private readonly IRepository<ProductMediaImageTypeMaster, long> _productMediaTypeRepository;
        private readonly IRepository<ProductBrandingPriceVariants, long> _productBrandingPriceVariantsRepository;
        private readonly IRepository<ProductVariantsData, long> _productVariantDataRepository;
        private readonly IRepository<ProductAssignedSubCategories, long> _productAssignedCategoriesRepository;
        private IDbConnection _db;
        private readonly IConfiguration _configuration;
        private readonly DbConnectionUtility _connectionUtility;
        private readonly IRepository<ProductVariantdataImages, long> _ProductVariantdataImagesRepository;
        private readonly IRepository<ProductBrandingPosition, long> _ProductBrandingPosition;
        private readonly IRepository<ProductAssignedCategoryMaster, long> _ProductAssignedCategoryMaster;
        private readonly IRepository<ProductAssignedSubSubCategories, long> _ProductAssignedSubSubCategories;
        public ProductFrontEndAppService(IRepository<ProductMaster, long> repository, IUnitOfWorkManager unitOfWorkManager, IRepository<UserBannerLogoData, long> userBannerLogoDataRepository,
            IRepository<CategoryMaster, long> categoryMasterRepository, IRepository<BrandingMethodMaster, long> repositoryBrandingMethod
            , IRepository<ProductImages, long> productImagesRepository, IRepository<ProductMediaImages, long> productMediaImagesRepository, IRepository<ProductMediaImageTypeMaster, long> productMediaTypeRepository, IRepository<ProductBrandingPriceVariants, long> productBrandingPriceVariantsRepository, IRepository<ProductVariantsData, long> productVariantDataRepository, IRepository<ProductBrandingPriceVariants,
                long> ProductBrandingPriceVariants,
            IRepository<ProductAssignedSubCategories, long> productAssignedCategoriesRepository,
             DbConnectionUtility connectionUtility, IConfiguration configuration, 
             IRepository<ProductVariantdataImages, long> ProductVariantdataImagesRepository, IRepository<ProductBrandingPosition, long> ProductBrandingPosition, IRepository<ProductAssignedCategoryMaster, long> ProductAssignedCategoryMaster, IRepository<ProductAssignedSubSubCategories, long> productAssignedSubSubCategories)
        {
            _repository = repository;
            _unitOfWorkManager = unitOfWorkManager;
            _userBannerLogoDataRepository = userBannerLogoDataRepository;
            _categoryMasterRepository = categoryMasterRepository;
            _repositoryBrandingMethod = repositoryBrandingMethod;
            _productImagesRepository = productImagesRepository;
            _productMediaImagesRepository = productMediaImagesRepository;
            _productMediaTypeRepository = productMediaTypeRepository;
            _productBrandingPriceVariantsRepository = productBrandingPriceVariantsRepository;
            _productVariantDataRepository = productVariantDataRepository;
            _productAssignedCategoriesRepository = productAssignedCategoriesRepository;
            _connectionUtility = connectionUtility;
            _configuration = configuration;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);
            _ProductVariantdataImagesRepository = ProductVariantdataImagesRepository;
            _ProductBrandingPosition = ProductBrandingPosition;
            _ProductAssignedCategoryMaster = ProductAssignedCategoryMaster;
            _ProductAssignedSubSubCategories = productAssignedSubSubCategories;
        }


        public async Task<ProductViewDto> GetProductListData(ProductMasterResultRequestDto Input)
        {
            IQueryable<ProductView> ProductList = Enumerable.Empty<ProductView>().AsQueryable();
            int TotalCount = 0;
            bool IsLoadMore = false;
            ProductViewDto response = new ProductViewDto();
            List<ProductView> ProductData = new List<ProductView>();
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

                using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
                {
                    await _connectionUtility.EnsureConnectionOpenAsync();

                    IEnumerable<ProductView> ProductDataa = await _db.QueryAsync<ProductView>("usp_GetProductListBySearch", new
                    {
                        SearchText = Input.SearchText,
                        TenantId = Convert.ToInt32(TenantId),
                        Type = Input.TypeId
                    }, commandType: System.Data.CommandType.StoredProcedure);

                    ProductData = ProductDataa.ToList();
                    List<CategoryCollections> CollectionList = new List<CategoryCollections>();


                    if (Input.CategoryId.HasValue)
                    {
                        //var CategoryData = (from cat in _categoryMasterRepository.GetAll()
                        //                    join grp in _CategoryGroupsRepository.GetAll() on cat.Id equals grp.CategoryMasterId
                        //                    where grp.CategoryGroupId == Input.CategoryGroupId.Value
                        //                    select cat).ToList();

                        ProductData = (from master in ProductData
                                       join assign in _ProductAssignedCategoryMaster.GetAll() on master.Id equals assign.ProductId
                                       where assign.CategoryId == Input.CategoryId.Value
                                       //join catgeory in CategoryData on assign.SubCategoryId equals catgeory.Id
                                       select master).ToList();


                    }
                    if (Input.SubCategoryIds != null)
                    {
                        ProductData = (from master in ProductData
                                       join assign in _productAssignedCategoriesRepository.GetAll() on master.Id equals assign.ProductId
                                       join category in Input.SubCategoryIds on assign.SubCategoryId equals category
                                       select master).ToList();
                    }

                    if (Input.SubSubCategoryIds != null)
                    {
                        ProductData = (from master in ProductData
                                       join assign in _ProductAssignedSubSubCategories.GetAll() on master.Id equals assign.ProductId
                                       join category in Input.SubSubCategoryIds on assign.SubSubCategoryId equals category
                                       select master).ToList();
                    }


                    TotalCount = ProductData.Count();
                    ProductData = ProductData.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).ToList();
                    ProductData = await ApplyDtoSortingProductViewList(ProductData, Input);
                    ProductData = ProductData.Skip(Input.SkipCount).Take(Input.MaxResultCount).ToList();

                    if (Input.SkipCount + Input.MaxResultCount < TotalCount)
                    {
                        IsLoadMore = true;
                    }

                    response.SkipCount = Input.SkipCount;
                    response.items = ProductData;
                    response.TotalCount = TotalCount;
                    response.IsLoadMore = IsLoadMore;
                }
            }
            catch (Exception ex)
            {
            }

            return response;
        }

        public async Task<List<ProductImageType>> GetBannerLogoData(string EncryptedTenantId, int? PageType, string PageTypeTitle, int? ImageType)
        {
            List<ProductImageType> BannerList = new List<ProductImageType>();
            Utility utility = new Utility();
            string TenantId = string.Empty;
            List<ProductImageType> BannerLogoData = new List<ProductImageType>();
            try
            {
                if (string.IsNullOrEmpty(EncryptedTenantId))
                {
                    int? SessionTenantId = AbpSession.TenantId;
                    if (SessionTenantId.HasValue)
                    {
                        TenantId = SessionTenantId.ToString();
                    }
                }
                else
                {
                    TenantId = utility.DecryptString(EncryptedTenantId);
                }
                if (string.IsNullOrEmpty(TenantId))
                {
                    throw new AbpAuthorizationException("Tenant user is unauthorized");
                }

                using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
                {

                    await _connectionUtility.EnsureConnectionOpenAsync();

                    IEnumerable<ProductImageType> ProductDataa = await _db.QueryAsync<ProductImageType>("usp_GetBannerLogoData", new
                    {
                        PageTypeTitle = PageTypeTitle,
                        TenantId = Convert.ToInt32(TenantId),
                        ImageType = ImageType,
                        PageType = PageType

                    }, commandType: System.Data.CommandType.StoredProcedure);

                    BannerLogoData = ProductDataa.ToList();


                }
                return BannerLogoData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task DeleteBannerlogoById(UserLogoBannerResultRequestDto Input)
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

                if (Input.Id > 0)
                {
                    var BannerLogoData = _userBannerLogoDataRepository.GetAllList(i => i.Id == Input.Id).FirstOrDefault();
                    if (BannerLogoData != null)
                    {
                        await _userBannerLogoDataRepository.DeleteAsync(BannerLogoData);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task<bool> DeleteUserBannerLogoData(List<UserBannerLogoData> UserBannerLogoData, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _userBannerLogoDataRepository.BulkDelete(UserBannerLogoData);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }


        #region sorting 
        private async Task<IQueryable<CollectionproductView>> ApplyDtoSorting(IQueryable<CollectionproductView> query, ProductMasterResultRequestDto input)
        {
            if (input.Sorting.HasValue)
            {
                switch (input.Sorting)
                {
                    case 1:
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.Id);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Id);
                        }

                        break;
                    case 2://
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.ProductListView.Select(i => i.ProductTitle));
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductListView.Select(i => i.ProductTitle));
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.ProductListView.Select(i => i.ProductTitle));
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductListView.Select(i => i.ProductTitle));
                        }
                        break;

                    case 4://price
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.ProductListView.Select(i => i.UnitPrice));
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductListView.Select(i => i.UnitPrice));
                        }
                        break;
                }
            }
            return query;
        }
        #endregion

        #region sorting 
        private async Task<IQueryable<ProductListViewDto>> ApplyDtoSortingDto(IQueryable<ProductListViewDto> query, ProductMasterResultRequestDto input)
        {
            if (input.Sorting.HasValue)
            {
                switch (input.Sorting)
                {
                    case 1:
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.Id);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Id);
                        }

                        break;
                    case 2://
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.ProductTitle);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle);
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.ProductTitle);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle);
                        }
                        break;

                    case 4://price
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.UnitPrice);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.UnitPrice);
                        }
                        break;
                }
            }
            return query;
        }

        private async Task<IQueryable<ProductView>> ApplyDtoSortingProductView(IQueryable<ProductView> query, ProductMasterResultRequestDto input)
        {
            if (input.Sorting.HasValue)
            {
                switch (input.Sorting)
                {
                    case 1:
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.Id);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Id);
                        }

                        break;
                    case 2://
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.ProductTitle);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle);
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.ProductTitle);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle);
                        }
                        break;

                    case 4://price
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.UnitPrice);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.UnitPrice);
                        }
                        break;
                }
            }
            return query;
        }

        private async Task<List<ProductView>> ApplyDtoSortingProductViewList(List<ProductView> query, ProductMasterResultRequestDto input)
        {
            if (input.Sorting.HasValue)
            {
                switch (input.Sorting)
                {
                    case 1:
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.Id).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Id).ToList();
                        }

                        break;
                    case 2://
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.ProductTitle).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle).ToList();
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.ProductTitle).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle).ToList();
                        }
                        break;

                    case 4://price
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.UnitPrice).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.UnitPrice).ToList();
                        }
                        break;
                    default:
                        {
                            query = query.OrderBy(x => x.ProductTitle).ToList();
                        }
                        break;
                }
            }
            return query;
        }


        #endregion

        #region Get product data by id
        public async Task<GetProductFrontEndDto> GetProductDataById(ProductDataRequestDto Input)
        {
            long[] ValuesArray;
            int IsProduct = 0;
            GetProductFrontEndDto ProductData = new GetProductFrontEndDto();
            string TenantId = string.Empty;
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

            try
            {
                var product = await _db.QueryFirstOrDefaultAsync<ProductCustomDataDto>("usp_GetProductDataById", new
                {
                    ProductId = Input.ProductId,
                    TenantId = TenantId
                }, commandType: System.Data.CommandType.StoredProcedure);

                if (product != null)
                {

                    IEnumerable<ProductImageType> ProductImages = await _db.QueryAsync<ProductImageType>(@"select images.ImageName as FileName, images.ImagePath,images.IsDefaultImage, images.Ext,images.[Name], images.[Name] as FileName,(case when images.[Size] IS NULL Then 0 else images.Size end) as Size,
	            images.[Type],images.Id, images.[Url], masters.ProductSKU  from ProductImages as images
                inner join ProductMaster as masters on masters.Id = images.ProductId where images.ProductId = " + Input.ProductId + " and images.TenantId = " + TenantId + " and images.IsDeleted=0", new
                    {
                        ProductId = Input.ProductId
                    }, commandType: System.Data.CommandType.Text);
                    IEnumerable<VariantDataColor> DefaultImageVariantData = await _db.QueryAsync<VariantDataColor>("Usp_GetDefaultImageVaiantData", new
                    {
                        ProductId = Input.ProductId,
                        TenantId = TenantId

                    }, commandType: System.Data.CommandType.StoredProcedure);

                    List<VariantModel> ColorVariantModel = new List<VariantModel>();
                    List<VariantModel> ColorVariantDefaultImageModel = new List<VariantModel>();
                    IEnumerable<ProductBrandingPositionDto> productBrandingPositionDtos = await _db.QueryAsync<ProductBrandingPositionDto>("usp_GetProductBrandingPositionByProductId", new
                    {
                        ProductId = Input.ProductId
                    }, commandType: System.Data.CommandType.StoredProcedure);

                    IEnumerable<ProductStockLocationDto> productStockLocationDtos = await _db.QueryAsync<ProductStockLocationDto>("usp_GetProductLocationByProductId", new
                    {
                        ProductId = Input.ProductId
                    }, commandType: System.Data.CommandType.StoredProcedure);

                    IEnumerable<ProductBrandingPriceVariantsDto> productBrandingPriceList = await _db.QueryAsync<ProductBrandingPriceVariantsDto>("usp_GetProductBrandingPriceByProductId", new
                    {
                        ProductId = Input.ProductId
                    }, commandType: System.Data.CommandType.StoredProcedure);

                    IEnumerable<BrandingMethodDto> brandingMethodList = await _db.QueryAsync<BrandingMethodDto>("usp_GetProductBrandingMethodDataByProductId", new
                    {
                        ProductId = Input.ProductId
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    IEnumerable<VariantDataColor> VariantData = await _db.QueryAsync<VariantDataColor>("usp_getProductVariantColor", new
                    {
                        ProductId = Input.ProductId,
                        TenantId = TenantId

                    }, commandType: System.Data.CommandType.StoredProcedure);


                    foreach (var data in VariantData)
                    {
                        VariantModel variant = new VariantModel();
                        List<VariantValues> VariantVals = new List<VariantValues>();
                        VariantValues SingleSize = new VariantValues();
                        string[] splitVals = data.VariantMasterIds.Split('/');
                        string[] splitVariant = data.Variant.Split('/');
                        var IsExists = splitVals.Where(i => i == "3").FirstOrDefault();
                        VariantModel CheckToBeExists = new VariantModel();
                        string color = string.Empty;
                        if (IsExists != null)
                        {
                            int? Index;
                            int position = Array.IndexOf(splitVals, "3");
                            if (position > -1)
                            {
                                Index = position;
                                color = splitVariant[Index.Value];
                                CheckToBeExists = ColorVariantModel.Where(i => i.Color.ToLower() == color.ToLower()).FirstOrDefault();
                                if (CheckToBeExists == null)
                                {
                                    variant.Id = data.Id;
                                    variant.Color = splitVariant[Index.Value];


                                }
                            }
                            var IsSizeExists = splitVals.Where(i => i == "1").FirstOrDefault();
                            if (IsSizeExists != null)
                            {
                                int? Ind;
                                int pos = Array.IndexOf(splitVals, "1");
                                if (pos > -1)
                                {
                                    Index = pos;
                                    if (CheckToBeExists == null)
                                    {
                                        SingleSize.Sizes = splitVariant[Index.Value];
                                        SingleSize.Price = data.Price;
                                        SingleSize.Quantity = data.QuantityStockUnit;
                                    }
                                    else
                                    {
                                        var SizeExists = ColorVariantModel.Where(i => i.Color.ToLower() == color.ToLower() && i.Variant.Any(i => i.Sizes.ToLower() == splitVariant[Index.Value].ToLower())).ToList();
                                        if (SizeExists.Count == 0)
                                        {
                                            ColorVariantModel.Where(i => i.Color.ToLower() == color.ToLower()).FirstOrDefault().Variant.Add(new VariantValues { Sizes = splitVariant[Index.Value], Price = data.Price, Quantity = data.QuantityStockUnit, Id = data.Id });
                                        }
                                    }
                                }
                            }
                        }
                        if (CheckToBeExists == null)
                        {
                            VariantVals.Add(SingleSize);
                            variant.Variant = VariantVals;
                            variant.ProductImage = data.ImageURL;
                            ColorVariantModel.Add(variant);
                        }
                    }
                    foreach (var data in DefaultImageVariantData)
                    {
                        VariantModel variantImage = new VariantModel();

                        if (data != null)
                        {
                            variantImage.ProductImage = data.ImageURL;
                            ColorVariantDefaultImageModel.Add(variantImage);
                        }
                    }
                    var VariantImages = await _db.QueryAsync<ProductImageType>("usp_GetVariantImagesByProductId", new
                    {
                        ProductId = Input.ProductId,
                        TenantId = TenantId
                    }, commandType: System.Data.CommandType.StoredProcedure);

                    IEnumerable<ProductImageType> ProductMediaImage = await _db.QueryAsync<ProductImageType>(@"select images.ImageName as FileName,images.ProductMediaImageTypeId as MediaType, images.ImageUrl as ImagePath,images.IsDefaultImage, images.Ext, images.[Name],(case when images.[Size] IS NULL Then 0 else images.Size end) as Size,
	                        images.[Type],images.Id, images.[Url]  from ProductMediaImages as images
	                        where images.ProductId = " + Input.ProductId + " and images.TenantId = " + Convert.ToInt32(TenantId) + " and images.IsDeleted=0", new
                    {
                        ProductId = Input.ProductId
                    }, commandType: System.Data.CommandType.Text);


                    IEnumerable<ProductVariantValueDto> ProductVariantData = await _db.QueryAsync<ProductVariantValueDto>("usp_GetFrontendVariantDataByProdId", new
                    {
                        ProductId = Input.ProductId,
                        TenantId = TenantId
                    }, commandType: System.Data.CommandType.StoredProcedure);

                    IEnumerable<ProductVolumeDiscountVariantDto> VolumeVariant = await _db.QueryAsync<ProductVolumeDiscountVariantDto>(@"select variant.Id, variant.ProductId, variant.QuantityFrom, variant.Price,variant.IsActive, 40 as 'ProfitMargin' from ProductVolumeDiscountVariant as variant
                    where variant.ProductId = " + Input.ProductId + " and variant.IsDeleted = 0 and variant.TenantId =" + Convert.ToInt32(TenantId) + "", new
                    {
                        ProductId = Input.ProductId
                    }, commandType: System.Data.CommandType.Text);

                    if (product.IsHasSubProducts == true)
                    {

                        IsProduct = 1;
                    }
                    IEnumerable<ProductBrandingPriceVsDto> ProductBrandingPriceV = await _db.QueryAsync<ProductBrandingPriceVsDto>("Usp_BrandingVariant", new
                    {
                        ProductId = Input.ProductId,
                        TenantId = TenantId,

                        IsHasSubProducts = product.IsHasSubProducts ? 1 : 0,
                        NumberOfSubProducts = product.NumberOfSubProducts,

                    }, commandType: System.Data.CommandType.StoredProcedure);

                    IEnumerable<ProductBrandingPriceVsDto> AdditionalBrandingPriceVs = await _db.QueryAsync<ProductBrandingPriceVsDto>("Usp_BrandingVariant", new
                    {
                        ProductId = Input.ProductId,
                        TenantId = TenantId,
                        IsHasSubProducts = product.IsHasSubProducts ? 1 : 0,
                        NumberOfSubProducts = product.NumberOfSubProducts,

                    }, commandType: System.Data.CommandType.StoredProcedure);
                    var ProductBrandingPriceVariantsData = ProductBrandingPriceV.ToList();
                    ProductData = new GetProductFrontEndDto()
                    {
                        Id = product.Id,
                        ProductSKU = product.ProductSKU,
                        ProductTitle = product.ProductTitle,
                        ProductDescripition = product.ProductDescripition,
                        ShortDescripition = product.ShortDescripition,
                        ProductNotes = product.ProductNotes,
                        IsActive = product.IsActive,
                        IsProductHasMultipleOptions = product.IsProductHasMultipleOptions,
                        IsPhysicalProduct = product.IsPhysicalProduct,
                        IsPublished = product.IsPublished,
                        IsIndentOrder = product.IsIndentOrder,
                        TurnAroundTime = product.TurnAroundTime,
                        Features = product.Features,
                        IsShowPriceWithoutAccount = product.IsShowPriceWithoutAccount,
                        IsProductIsCompartmentType = product.IsProductCompartmentType,
                        IsProductHasCompartmentBuilder = product.IsProductHasCompartmentBuilder,
                        BrandingUnitOfMeasure = product.BrandingUnitOfMeasure,
                        NumberOfPieces = product.NumberOfPieces,
                        BrandingUnitOfMeasureId = Convert.ToString(product.BrandingUnitOfMeasureId) == "" ? 0 : product.BrandingUnitOfMeasureId,
                        ProductPrice = (new ProductPriceDto
                        {
                            Id = product.Id,
                            Profit = Convert.ToString(product.Profit),
                            UnitPrice = Convert.ToString(product.UnitPrice),
                            CostPerItem = Convert.ToString(product.CostPerItem),
                            MarginIncreaseOnSalePrice = Convert.ToString(product.MarginIncreaseOnSalePrice),
                            SalePrice = Convert.ToString(product.SalePrice),
                            OnSale = product.OnSale,
                            UnitOfMeasure = Convert.ToString(product.UnitOfMeasure),
                            MinimumOrderQuantity = Convert.ToString(product.MinimumOrderQuantity),
                            DepositRequired = Convert.ToString(product.DepositRequired),
                            ChargeTaxOnThis = product.ChargeTaxOnThis,
                            ProductHasPriceVariant = product.ProductHasPriceVariant,
                            DiscountPercentage = Convert.ToString(product.DiscountPercentage),
                            DiscountPercentageDraft = Convert.ToString(product.DiscountPercentageDraft),
                            MinimumSalePrice = VolumeVariant.ToList().Count > 0 ? VolumeVariant.Where(i => i.Price > 0).Select(i => i.Price).Min() : Convert.ToDecimal(product.SalePrice)
                        }),

                        //ProductArtWork
                        ProductArtWorkDto = new ProductArtWorkDto
                        {
                            ArtWorkActualUnitPrice = product.IsArtworkEnabled == false ? 0 : product.ArtWorkUnitPrice,
                            ArtWorkUnitPrice = product.IsArtworkEnabled == false ? 0 : (product.IsHasSubProducts == true ? (product.NumberOfSubProducts > 0 ? product.ArtWorkUnitPrice * product.NumberOfSubProducts : product.ArtWorkUnitPrice) : product.ArtWorkUnitPrice),
                            ArtWorkHandlingCharges = (product.IsArtworkEnabled) == false ? 0 : product.ArtWorkHandlingCharges,
                        },
                        SubProductDto = new SubProductDto
                        {
                            NumberOfSubProducts = product.IsHasSubProducts == true ? product.NumberOfSubProducts : 0,
                            IsHasSubProducts = product.IsHasSubProducts,
                        },


                        //ProductDetail
                        ProductDetail = new ProductDetailFrontendComboDto
                        {
                            BrandStringArray = product.BrandStringArray,
                            TypeStringArray = product.TypeStringArray,
                            MaterialStringArray = product.MaterialStringArray,
                            CollectionStringArray = product.CollectionStringArray,
                            TagStringArray = product.TagStringArray,
                            VendorStringArray = product.VendorStringArray
                        },

                        //Product Dimenstions Inventory
                        ProductDimensionsInventory = new ProductDiamensionsCombo
                        {

                            Id = product.InventoryId,
                            ProductId = product.Id,
                            PalletDimension = new PalletDimension
                            {
                                Id = product.InventoryId,
                                PalletWeight = product.PalletWeight,
                                CartonPerPallet = Convert.ToString(product.CartonPerPallet),
                                UnitPerPallet = Convert.ToString(product.UnitPerPallet),
                                PalletNote = product.PalletNote,
                                IsActive = true
                            },
                            CartonDimension = new CartonDimension
                            {
                                Id = product.InventoryId,
                                CartonHeight = product.CartonHeight,
                                CartonWidth = product.CartonWidth,
                                CartonLength = product.CartonLength,
                                CartonPackaging = product.CartonPackaging,
                                CartonWeight = product.CartonWeight,
                                CartonNote = product.CartonNote,
                                CartonCubicWeightKG = product.CartonCubicWeightKG,
                                CartonUnitOfMeasureId = product.CartonUnitOfMeasureId.ToString() == "" ? 0 : product.CartonUnitOfMeasureId,
                                CartonWeightMeasureId = product.CartonWeightMeasureId.ToString() == "" ? 0 : product.CartonWeightMeasureId,
                                CartonUnitOfMeasureTitle = product.CartonUnitOfMeasureTitle,
                                CartonWeightMeasureTitle = product.CartonWeightMeasureTitle
                            },

                            UnitDimension = new UnitDimensions
                            {
                                Id = product.InventoryId,
                                ProductHeight = product.ProductHeight,
                                ProductWidth = product.ProductWidth,
                                ProductLength = product.ProductLength,
                                UnitWeight = product.UnitWeight,
                                ProductPackaging = product.ProductPackaging,
                                UnitPerProduct = Convert.ToString(product.UnitPerProduct),
                                ProductDiameter = product.ProductDiameter,
                                ProductDimensionNotes = product.ProductDimensionNotes,
                                ProductUnitMeasureId = product.ProductUnitMeasureId.ToString() == "" ? 0 : product.ProductUnitMeasureId,
                                ProductWeightMeasureId = product.ProductWeightMeasureId.ToString() == "" ? 0 : product.ProductWeightMeasureId,
                                ProductUnitMeasureTitle = product.ProductUnitMeasureTitle,
                                ProductWeightMeasureTitle = product.ProductWeightMeasureTitle
                            },

                        },

                        //ProductBrandingPosition
                        ProductBrandingPosition = productBrandingPositionDtos.ToList(),


                        //ProductStockLocation
                        ProductStockLocation = productStockLocationDtos.ToList(),

                        //ProductBrandingPrice
                        ProductBrandingPrice = productBrandingPriceList.ToList(),

                        //Branding Method Data
                        BrandingMethodData = brandingMethodList.ToList(),


                        ProductMediaImages = (from type in _productMediaTypeRepository.GetAllList(x => x.IsActive == true)
                                              select new ProductMediaModel
                                              {
                                                  MediaType = type.Id,
                                                  MediaTypeName = type.ProductMediaImageTypeName,
                                                  Images = (from image in ProductMediaImage.Where(i => i.MediaType == type.Id)
                                                            select new ProductImageType
                                                            {
                                                                Id = image.Id,
                                                                MediaType = image.MediaType,
                                                                ImagePath = image.ImagePath,
                                                                FileName = image.FileName,
                                                                Ext = image.Ext,
                                                                Size = image.Size,
                                                                Type = image.Type,
                                                                Name = image.Name,
                                                                Url = image.ImagePath
                                                            }).ToArray()

                                              }).ToList(),

                        ProductVariantValues = ProductVariantData.ToList(),


                        //ProductVariantImages
                        ProductMainAndVariantImages = ProductImages.Where(i => i.IsDefaultImage == true).Concat(VariantImages).ToList(),
                        //VariantImages.Concat(ProductImages.Where(i => i.IsDefaultImage == true)).ToList(),

                        //ProductImagesNames
                        ProductImagesNames = ProductImages.Where(i => i.IsDefaultImage == false).ToList(),

                        VariantColorSizeModel = ColorVariantDefaultImageModel.Where(i => i.ProductImage != "").Concat(ColorVariantModel).ToList()
                        ,



                        ProductBrandingPriceVariants = (from branding in ProductBrandingPriceVariantsData
                                                        select new BrandingMethodPriceModel
                                                        {
                                                            Id = branding.BrandingId,
                                                            MethodName = branding.MethodName,
                                                            UnitPrice = branding.UnitPrice,
                                                            IsColorSelected = branding.IsColorSelected,
                                                            SelectedColor = branding.SelectedColor,
                                                            MaxSelectedColor = string.IsNullOrEmpty(branding.MaxSelectedColor) ? "0" : branding.MaxSelectedColor,

                                                            PriceModel = (from price in ProductBrandingPriceV.ToList()
                                                                          where price.BrandingId == branding.BrandingId
                                                                          select new ProductBrandingPriceVariantsDto
                                                                          {
                                                                              Id = price.Id,
                                                                              BrandingMethodId = price.BrandingMethodId,
                                                                              Quantity = price.Quantity,
                                                                              Price = price.Price,
                                                                              BrandingUnitPrice = price.BrandingUnitPrice,
                                                                              IsActive = price.IsActive,

                                                                          }).ToList()
                                                        }).ToList(),

                        AdditionalBrandingPriceVariants = (from branding in ProductBrandingPriceVariantsData

                                                           select new BrandingMethodPriceModel
                                                           {
                                                               Id = branding.BrandingId,
                                                               MethodName = branding.MethodName,
                                                               UnitPrice = branding.UnitPrice,
                                                               IsColorSelected = branding.IsColorSelected,
                                                               SelectedColor = branding.SelectedColor,
                                                               MaxSelectedColor = string.IsNullOrEmpty(branding.MaxSelectedColor) ? "0" : branding.MaxSelectedColor,

                                                               PriceModel = (from price in ProductBrandingPriceV.ToList()
                                                                             where price.BrandingId == branding.BrandingId
                                                                             select new ProductBrandingPriceVariantsDto
                                                                             {
                                                                                 Id = price.Id,
                                                                                 BrandingMethodId = price.BrandingMethodId,
                                                                                 Quantity = price.Quantity,
                                                                                 Price = price.Price,
                                                                                 BrandingUnitPrice = price.BrandingUnitPrice,
                                                                                 IsActive = price.IsActive,

                                                                             }).ToList()
                                                           }).ToList(),

                        ProductDefaultImage = ProductImages.Where(i => i.IsDefaultImage == true).FirstOrDefault(),

                        //ProductBrandingPriceVariants = (from branding in _repositoryBrandingMethod.GetAll().AsQueryable()
                        //                                join method in _productBrandingMethodsRepository.GetAll().AsQueryable() on branding.Id equals method.BrandingMethodId
                        //                                where method.ProductMasterId == Input.ProductId
                        //                                select new BrandingMethodPriceModel
                        //                                {
                        //                                    Id = branding.Id,
                        //                                    MethodName = branding.MethodName,
                        //                                    UnitPrice = (product.IsHasSubProducts == true ? (product.NumberOfSubProducts > 0 ? (branding.UnitPrice * product.NumberOfSubProducts) : branding.UnitPrice) : branding.UnitPrice),
                        //                                    // IsColorSelected = _BrandingMethodAssignedColors.GetAll().Where(i => i.BrandingMethodId == branding.Id).Select(i => i.Id).FirstOrDefault() > 0 ? true : false,
                        //                                    IsColorSelected = branding.ColorSelectionType == 1 ? true : false,
                        //                                    SelectedColor = "1", // sending hardcode 1 because product will have atleast 1 quantity bydefault required by FE
                        //                                    MaxSelectedColor = string.IsNullOrEmpty((from color in _BrandingMethodAssignedColors.GetAll().Where(i => i.BrandingMethodId == branding.Id)
                        //                                                                             join colormaster in _ProductColourMasterRepo.GetAll() on color.ColorId equals colormaster.Id
                        //                                                                             select colormaster.ProductColourName
                        //                                                     ).FirstOrDefault()) ? "0" : (from color in _BrandingMethodAssignedColors.GetAll().Where(i => i.BrandingMethodId == branding.Id)
                        //                                                                                  join colormaster in _ProductColourMasterRepo.GetAll() on color.ColorId equals colormaster.Id
                        //                                                                                  select colormaster.ProductColourName
                        //                                                     ).FirstOrDefault(),

                        //                                    PriceModel = (from price in _productBrandingPriceVariantsRepository.GetAll().Where(i => i.BrandingMethodId == branding.Id)
                        //                                                  select new ProductBrandingPriceVariantsDto
                        //                                                  {
                        //                                                      Quantity = price.Quantity,
                        //                                                      BrandingUnitPrice = price.BrandingUnitPrice,
                        //                                                      IsActive = price.IsActive,
                        //                                                      Price = price.Price,
                        //                                                      Id = price.Id,
                        //                                                      BrandingMethodId = price.BrandingMethodId
                        //                                                  }).ToList()
                        //                                }).ToList(),


                        //AdditionalBrandingPriceVariants = (from branding in _repositoryBrandingMethod.GetAll().AsQueryable()
                        //                                   join method in _productBrandingMethodsRepository.GetAll().AsQueryable() on branding.Id equals method.BrandingMethodId
                        //                                   where method.ProductMasterId == Input.ProductId
                        //                                   select new BrandingMethodPriceModel
                        //                                   {
                        //                                       Id = branding.Id,
                        //                                       MethodName = branding.MethodName,
                        //                                       UnitPrice = (product.IsHasSubProducts == true ? (product.NumberOfSubProducts > 0 ? (branding.UnitPrice * product.NumberOfSubProducts) : branding.UnitPrice) : branding.UnitPrice),

                        //                                       MaxSelectedColor = string.IsNullOrEmpty((from color in _BrandingMethodAssignedColors.GetAll().Where(i => i.BrandingMethodId == branding.Id)
                        //                                                                                join colormaster in _ProductColourMasterRepo.GetAll() on color.ColorId equals colormaster.Id
                        //                                                                                select colormaster.ProductColourName
                        //                                                     ).FirstOrDefault()) ? "0" : (from color in _BrandingMethodAssignedColors.GetAll().Where(i => i.BrandingMethodId == branding.Id)
                        //                                                                                  join colormaster in _ProductColourMasterRepo.GetAll() on color.ColorId equals colormaster.Id
                        //                                                                                  select colormaster.ProductColourName
                        //                                                     ).FirstOrDefault(),
                        //                                       SelectedColor = "1", // sending hardcode 1 because product will have atleast 1 quantity bydefault required by FE
                        //                                       IsColorSelected = branding.ColorSelectionType == 1 ? true : false,


                        //                                       PriceModel = (from price in _productBrandingPriceVariantsRepository.GetAll().Where(i => i.BrandingMethodId == branding.Id)
                        //                                                     select new ProductBrandingPriceVariantsDto
                        //                                                     {
                        //                                                         Quantity = price.Quantity,
                        //                                                         BrandingUnitPrice = price.BrandingUnitPrice,
                        //                                                         IsActive = price.IsActive,
                        //                                                         Price = price.Price,
                        //                                                         Id = price.Id,
                        //                                                         BrandingMethodId = price.BrandingMethodId
                        //                                                     }).ToList()
                        //                                   }).ToList(),

                        ProductVolumeDiscountVariant = VolumeVariant.Where(i => i.Price > 0 && i.QuantityFrom > 0).OrderBy(i => i.QuantityFrom).ToList()
                    };
                }
            }
            catch (Exception ex)
            {

            }

            return ProductData;
        }


        #endregion



        private static async Task<string> GetColorVariantValue(long ColorId, List<ProductVariantOptionValues> ValuesList)
        {
            string ColorValue = string.Empty;
            var IsSizeExists = ValuesList.Where(i => i.VariantOptionId == ColorId).FirstOrDefault();
            if (IsSizeExists != null)
            {
                ColorValue = IsSizeExists.VariantOptionValue;
            }
            return ColorValue;
        }

        private static async Task<long[]> ConvertToLongArray(string Value)
        {
            long[] Response = new long[] { };
            try
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    string SeparatedValues = Value.Replace("'", "");
                    SeparatedValues = SeparatedValues.Trim();
                    Response = SeparatedValues.Split(',').Select(long.Parse).ToArray();
                }
            }
            catch (Exception ex)
            {

            }
            return Response;

        }
        private static async Task<string> GetSizeVariantValue(long SizeId, List<ProductVariantOptionValues> ValuesList)
        {
            string SizeValue = string.Empty;
            var IsSizeExists = ValuesList.Where(i => i.VariantOptionId == SizeId).FirstOrDefault();
            if (IsSizeExists != null)
            {
                SizeValue = IsSizeExists.VariantOptionValue;
            }
            return SizeValue;
        }


        public async Task<FrontendMediaImagesDto> GetTenantProductMediaImages(FrontEndMediaFilter Input)
        {
            Utility utility = new Utility();

            FrontendMediaImagesDto Response = new FrontendMediaImagesDto();
            string TenantId = string.Empty;
            var OtherMediaImageId = _productMediaTypeRepository.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "othermediatype").FirstOrDefault();
            var LifeStyleImagesId = _productMediaTypeRepository.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "lifestyleimages").FirstOrDefault();
            var LineArtId = _productMediaTypeRepository.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "lineart").FirstOrDefault();

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

            using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
            {

                var ProductMediaImages = _productMediaImagesRepository.GetAll();

                Response.LineArtImages = (from image in ProductMediaImages
                                          where image.ProductMediaImageTypeId == LineArtId.Id
                                          select new ProductImageType
                                          {
                                              Id = image.Id,
                                              Url = image.ImageUrl,
                                              FileName = image.ImageName,
                                              Size = image.Size,
                                              Type = image.Type,
                                              ImagePath = image.ImageUrl,
                                              Ext = image.Ext,
                                              Name = image.ImageName,
                                          }).ToList();
                Response.OtherMediaImages = (from image in ProductMediaImages
                                             where image.ProductMediaImageTypeId == OtherMediaImageId.Id
                                             select new ProductImageType
                                             {
                                                 Id = image.Id,
                                                 Url = image.ImageUrl,
                                                 FileName = image.ImageName,
                                                 Size = image.Size,
                                                 Type = image.Type,
                                                 ImagePath = image.ImageUrl,
                                                 Ext = image.Ext,
                                                 Name = image.ImageName,
                                             }).ToList();
                Response.LifeStyleImages = (from image in ProductMediaImages
                                            where image.ProductMediaImageTypeId == LifeStyleImagesId.Id
                                            select new ProductImageType
                                            {
                                                Id = image.Id,
                                                Url = image.ImageUrl,
                                                FileName = image.ImageName,
                                                Size = image.Size,
                                                Type = image.Type,
                                                ImagePath = image.ImageUrl,
                                                Ext = image.Ext,
                                                Name = image.ImageName,
                                            }).ToList();
                Response.AllProductImages = (from image in _productImagesRepository.GetAll()
                                             select new ProductImageType
                                             {
                                                 Id = image.Id,
                                                 Url = image.ImagePath,
                                                 FileName = image.ImageName,
                                                 Size = image.Size,
                                                 Type = image.Type,
                                                 ImagePath = image.ImagePath,
                                                 Ext = image.Ext,
                                                 Name = image.ImageName,
                                             }).ToList();
            }
            return Response;
        }


        public async Task<ImagesCustomlistDto> GetAllProductImagesForTenant(FrontEndMediaFilter Input)
        {
            Utility utility = new Utility();

            ImagesCustomlistDto Response = new ImagesCustomlistDto();
            string TenantId = string.Empty;

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
            int TotalCount = 0;
            bool IsLoadMore = false;
            using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
            {

                var ProductMediaImages = _productMediaImagesRepository.GetAll();

                var ProductImages = (from image in ProductMediaImages
                                     where image.ImageUrl != null
                                     select new ProductImageType
                                     {
                                         Id = image.Id,
                                         Url = image.ImageUrl,
                                         FileName = image.ImageName,
                                         Size = image.Size,
                                         Type = image.Type,
                                         ImagePath = image.ImageUrl,
                                         Ext = image.Ext,
                                         Name = image.ImageName,
                                     }).ToList();

                TotalCount = ProductImages.Count();
                ProductImages = ProductImages.Skip(Input.SkipCount).Take(Input.MaxResultCount).OrderByDescending(i => i.Id).ToList();

                if (Input.SkipCount + Input.MaxResultCount < TotalCount)
                {
                    IsLoadMore = true;
                }

                Response.SkipCount = Input.SkipCount;
                Response.items = ObjectMapper.Map<List<ProductImageType>>(ProductImages);
                Response.TotalCount = TotalCount;
                Response.IsLoadMore = IsLoadMore;

            }
            return Response;
        }


        private async Task<bool> DeleteAsync(List<UserBannerLogoData> BannerData, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _userBannerLogoDataRepository.BulkDelete(BannerData);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        public async Task<List<BrandingMethodDto>> GetAllBrandingMethods(FrontEndMediaFilter Input)
        {
            Utility utility = new Utility();
            string TenantId = string.Empty;
            List<BrandingMethodDto> BrandingMethods = new List<BrandingMethodDto>();

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

            using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
            {
                try
                {
                    var BrandingMethodData = _repositoryBrandingMethod.GetAll();
                    BrandingMethods = (from method in BrandingMethodData
                                       select new BrandingMethodDto
                                       {
                                           Id = method.Id,
                                           MethodName = method.MethodName,
                                           Description = method.MethodDescripition,
                                           ImageUrl = method.ImageUrl,
                                           Ext = method.Ext,
                                           Url = method.Url,
                                           Name = method.Name,
                                           Type = method.Type,
                                           Size = method.Size
                                       }).ToList();

                }
                catch (Exception ex)
                {

                }
            }
            return BrandingMethods;
        }

        public async Task<CompartmentFrontEndCustomDto> GetCompartmetDataByProductId(FrontEndMediaFilter Input)
        {

            Utility utility = new Utility();
            string TenantId = string.Empty;

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

            List<CompartmentVariantCustomDataDto> CompartmentVariantList = new List<CompartmentVariantCustomDataDto>();
            IEnumerable<CompartmentVariantCustomDataDto> CompartmentVariantData = await _db.QueryAsync<CompartmentVariantCustomDataDto>("usp_GetCompartmentData", new
            {
                productId = Input.ProductId,
                tenantId = TenantId
            }, commandType: System.Data.CommandType.StoredProcedure);

            IEnumerable<ProductImageType> CompartmentBaseImage = await _db.QueryAsync<ProductImageType>(@"select  Url,Name,Type, Ext ,Size ,ImageFileName as FileName,ImagePath 
						from ProductCompartmentBaseImages  where ProductId = " + Input.ProductId + " and IsDeleted = 0 and TenantId =" + TenantId + "order by Id desc", new
            {
                ProductId = Input.ProductId,
            }, commandType: System.Data.CommandType.Text);

            CompartmentVariantList = CompartmentVariantData.GroupBy(g => g.CompartmentTitle).Select(x => x.FirstOrDefault()).Distinct().ToList();
            var CompartmentVariant = (from compartmentList in CompartmentVariantList
                                      select new CompartmentVariantDataDto
                                      {
                                          Id = compartmentList.Id,
                                          ProductId = compartmentList.ProductId,
                                          CompartmentTitle = compartmentList.CompartmentTitle,
                                          CompartmentSubTitle = compartmentList.CompartmentSubTitle,
                                          selectedCompartmentVariantImg = null,
                                          SKU = compartmentList.SKU,
                                          ProductName = compartmentList.ProductName,
                                          ProductTitle = compartmentList.ProductTitle,
                                          VarientList = (from item in CompartmentVariantData.ToList()
                                                         where item.CompartmentTitle.ToLower().Trim() == compartmentList.CompartmentTitle.ToLower().Trim()
                                                         select new VarientList
                                                         {
                                                             Id = item.ProductVarientId.Value,
                                                             Price = item.Price,
                                                             SKU = item.SKU,
                                                             ProductVarientId = item.ProductVarientId.Value,
                                                             Color = item.Color,
                                                             Variant = item.Variant,
                                                             ImageObj = new ProductImageType
                                                             {
                                                                 Ext = item.Ext,
                                                                 Url = item.Url,
                                                                 FileName = item.ImageFileName,
                                                                 ImagePath = item.ImagePath,
                                                                 Size = item.Size,
                                                                 Name = item.Name,
                                                                 Type = item.Type,
                                                                 BaseImageByteData = AsyncHelper.RunSync(() => ConvertToBase64(item.ImagePath)),
                                                             }
                                                         }).ToList()

                                      }).OrderBy(i => i.CompartmentTitle).ToList();


            string BaseImageData = string.Empty;

            string url = CompartmentBaseImage.Select(i => i.ImagePath).FirstOrDefault();
            if (url != null)
            {
                string encodedUrl = Convert.ToBase64String(Encoding.Default.GetBytes(url));

                using (var client = new WebClient())
                {
                    byte[] dataBytes = client.DownloadData(new Uri(url));
                    BaseImageData = Convert.ToBase64String(dataBytes);
                }
            }


            var CompartmentBaseImageData = (from product in CompartmentBaseImage
                                            select new ProductImageType
                                            {
                                                Url = product.Url,
                                                Type = product.Type,
                                                Size = product.Size,
                                                Name = product.Name,
                                                Ext = product.Ext,
                                                FileName = product.FileName,
                                                ImagePath = product.ImagePath,
                                                BaseImageByteData = BaseImageData
                                            }).FirstOrDefault();

            CompartmentFrontEndCustomDto list = new CompartmentFrontEndCustomDto();
            list.CompartmentVariantDataDto = CompartmentVariant;
            list.BaseImage = CompartmentBaseImageData;
            return list;
        }

        public async Task<string> ConvertToBase64(string FilePath)
        {
            string BaseImageData = string.Empty;
            string url = FilePath;
            try
            {
                string encodedUrl = Convert.ToBase64String(Encoding.Default.GetBytes(url));

                using (var client = new WebClient())
                {
                    byte[] dataBytes = client.DownloadData(new Uri(url));
                    BaseImageData = Convert.ToBase64String(dataBytes);
                }
            }
            catch (Exception ex)
            {

            }

            return BaseImageData;
        }

        public async Task<BaseImageArray> GetBaseImagesByProductId(BaseImageFilter Input)
        {
            Utility utility = new Utility();
            string TenantId = string.Empty;

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
            List<BaseImageData> ProductImagesResponse = new List<BaseImageData>();
            BaseImageArray Response = new BaseImageArray();
            try
            {

                var OtherMediaImageId = _productMediaTypeRepository.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "othermediatype").FirstOrDefault();
                var LifeStyleImagesId = _productMediaTypeRepository.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "lifestyleimages").FirstOrDefault();
                var LineArtId = _productMediaTypeRepository.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "lineart").FirstOrDefault();


                if (Input.FlagType == 1 && Input.IsdefaultImage == true)
                {
                    var ProductImage = _productImagesRepository.GetAllList(i => i.ProductId == Input.ProductId && i.IsDefaultImage == true).ToList();
                    Response.BaseImageData = (from image in ProductImage
                                              select new BaseImageData
                                              {
                                                  BaseImageString = AsyncHelper.RunSync(() => ConvertToBase64(image.ImagePath)),
                                                  ProductId = Input.ProductId
                                              }).ToList();
                }
                else if (Input.FlagType == 1 && Input.IsdefaultImage == false)
                {
                    var ProductImage = _productImagesRepository.GetAllList(i => i.ProductId == Input.ProductId && i.IsDefaultImage == false).ToList();
                    var VariantData = _productVariantDataRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
                    var PositionsData = _ProductBrandingPosition.GetAllList(i => i.ProductId == Input.ProductId).ToList();

                    try
                    {
                        Response.BaseImageData = (from image in ProductImage
                                                  select new BaseImageData
                                                  {
                                                      BaseImageString = AsyncHelper.RunSync(() => ConvertToBase64(image.ImagePath)),
                                                      ProductId = Input.ProductId,

                                                  }).ToList();

                        Response.VariantsBaseImageArray = (from variant in VariantData
                                                           join images in _ProductVariantdataImagesRepository.GetAllList() on variant.Id equals images.ProductVariantId
                                                           select AsyncHelper.RunSync(() => ConvertToBase64(images.ImageURL))
                                                           ).ToArray();
                        Response.PositionBaseImageArray = (from variant in PositionsData
                                                           select AsyncHelper.RunSync(() => ConvertToBase64(variant.ImageFileURL))
                                                           ).ToArray();
                    }
                    catch (Exception ex)
                    {

                    }

                }
                else if (Input.FlagType == LineArtId.Id)
                {
                    // line art
                    var ProductImage = _productMediaImagesRepository.GetAllList(i => i.ProductId == Input.ProductId && i.ProductMediaImageTypeId == LineArtId.Id).ToList();
                    Response.BaseImageData = (from image in ProductImage
                                              select new BaseImageData
                                              {
                                                  BaseImageString = AsyncHelper.RunSync(() => ConvertToBase64(image.ImageUrl)),
                                                  ProductId = Input.ProductId
                                              }).ToList();

                }
                else if (Input.FlagType == LifeStyleImagesId.Id)
                {
                    // life style
                    var ProductImage = _productMediaImagesRepository.GetAllList(i => i.ProductId == Input.ProductId && i.ProductMediaImageTypeId == LifeStyleImagesId.Id).ToList();
                    Response.BaseImageData = (from image in ProductImage
                                              select new BaseImageData
                                              {
                                                  BaseImageString = AsyncHelper.RunSync(() => ConvertToBase64(image.ImageUrl)),
                                                  ProductId = Input.ProductId
                                              }).ToList();

                }
                else if (Input.FlagType == OtherMediaImageId.Id)
                {
                    // other media
                    var ProductImage = _productMediaImagesRepository.GetAllList(i => i.ProductId == Input.ProductId && i.ProductMediaImageTypeId == OtherMediaImageId.Id).ToList();
                    Response.BaseImageData = (from image in ProductImage
                                              select new BaseImageData
                                              {
                                                  BaseImageString = AsyncHelper.RunSync(() => ConvertToBase64(image.ImageUrl)),
                                                  ProductId = Input.ProductId
                                              }).ToList();

                }

            }
            catch (Exception ex)
            {

            }
            return Response;
        }
    }
}
