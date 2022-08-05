using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using CaznerMarketplaceBackendApp.ECatalogue.Dto;
using CaznerMarketplaceBackendApp.MultiTenancy;
using CaznerMarketplaceBackendApp.Product.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CaznerMarketplaceBackendApp.AppConsts;

namespace CaznerMarketplaceBackendApp.ECatalogue
{
    public class ECatalogueAppService:
          AsyncCrudAppService<ECatalogueMaster, ECatalogueDto, long, ECatalogueRequestDto, CreateOrUpdateECatalogue, ECatalogueDto>, ICatalogueAppService

    {
        private readonly IRepository<ECatalogueMaster, long> _repository;
        private readonly IRepository<Tenant, int> _tenantManager;
        string AzureProductFolder = string.Empty;
        string AzureStorageUrl = string.Empty;
        private IConfiguration _configuration;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public ECatalogueAppService(IRepository<ECatalogueMaster, long> repository, IRepository<Tenant, int> tenantManager, IConfiguration configuration, IUnitOfWorkManager unitOfWorkManager) : base(repository)
        {
            _repository = repository;
            _tenantManager = tenantManager;
            _configuration = configuration;
            AzureProductFolder = _configuration["FileUpload:AzureProductFolder"];
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override async Task<ECatalogueDto> CreateAsync(CreateOrUpdateECatalogue Model)
        {
            ECatalogueDto Response = new ECatalogueDto();
            try
            {
                var Obj = ObjectMapper.Map<ECatalogueMaster>(Model);
                Obj.TenantId = AbpSession.TenantId.Value;
                if (Model.ImageObj != null)
                {
                    int TenantId = AbpSession.TenantId.Value;
                    var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
                    string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
                    string ImageLocation = AzureStorageUrl + folderPath + Model.ImageObj.Name;

                    Obj.Name = Model.ImageObj.Name;
                    Obj.Ext = Model.ImageObj.Ext;
                    Obj.Url = ImageLocation; 
                    Obj.Size = Model.ImageObj.Size;
                    Obj.Type = Model.ImageObj.Type;
                    Obj.ImageUrl = ImageLocation;
                }
                await _repository.InsertAsync(Obj);
                Response = ObjectMapper.Map<ECatalogueDto>(Obj);
            }
            catch(Exception ex)
            {

            }
            return Response;
        }
          
        public override async Task<ECatalogueDto> UpdateAsync(ECatalogueDto Model)
        {
            ECatalogueDto Response = new ECatalogueDto();
            try
            {
                var Obj = ObjectMapper.Map<ECatalogueMaster>(Model);
                Obj.TenantId = AbpSession.TenantId.Value;
                if (Model.ImageObj != null)
                {
                    int TenantId = AbpSession.TenantId.Value;
                    var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
                    string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
                    string ImageLocation = AzureStorageUrl + folderPath + Model.ImageObj.Name;

                    Obj.Name = Model.ImageObj.Name;
                    Obj.Ext = Model.ImageObj.Ext;
                    Obj.Url = ImageLocation;
                    Obj.Size = Model.ImageObj.Size;
                    Obj.Type = Model.ImageObj.Type;
                    Obj.ImageUrl = ImageLocation;
                }
                await _repository.UpdateAsync(Obj);
                Response = ObjectMapper.Map<ECatalogueDto>(Obj);
            }
            catch (Exception ex)
            {

            }
            return Response;
        }

        public override async Task<PagedResultDto<ECatalogueDto>> GetAllAsync(ECatalogueRequestDto Input)
        {
            Utility utility = new Utility();
            int TotalCount = 0;
            List<ECatalogueDto> ECatalogueData = new List<ECatalogueDto>();
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

                    var Data = _repository.GetAll();
                    ECatalogueData = (from obj in Data
                                      select new ECatalogueDto
                                      {
                                          Id = obj.Id,
                                          Title = obj.Title,
                                          ImageUrl = obj.ImageUrl,
                                          CatalogueUrl = obj.CatalogueUrl,
                                          ImageObj = new ProductMediaType {
                                              Ext = obj.Ext,
                                              Name = obj.Name,
                                              FileName = obj.Name,
                                              Size = obj.Size,
                                              Type = obj.Type,
                                              Url = obj.Url
                                          }
                                      }).ToList();

                    TotalCount = ECatalogueData.Count();

                    ECatalogueData = await ApplyDtoSorting(ECatalogueData.ToList(), Input);
                }
            }
            catch(Exception ex)
            {

            }
            return new PagedResultDto<ECatalogueDto>(TotalCount, ObjectMapper.Map<List<ECatalogueDto>>(ECatalogueData));
        }

        public async Task<CustomECatalogueDto> GetAllECatalogues(ECatalogueRequestDto Input)
        {
            Utility utility = new Utility();
            int TotalCount = 0;
            bool IsLoadMore = false;
            List<ECatalogueDto> ECatalogueData = new List<ECatalogueDto>();
            CustomECatalogueDto customECatalogue = new CustomECatalogueDto();
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

                    var Data = _repository.GetAll();
                    ECatalogueData = (from obj in Data
                                      select new ECatalogueDto
                                      {
                                          Id = obj.Id,
                                          Title = obj.Title,
                                          ImageUrl = obj.ImageUrl,
                                          CatalogueUrl = obj.CatalogueUrl,
                                          ImageObj = new ProductMediaType
                                          {
                                              Ext = obj.Ext,
                                              Name = obj.Name,
                                              FileName = obj.Name,
                                              Size = obj.Size,
                                              Type = obj.Type,
                                              Url = obj.Url
                                          }
                                      }).ToList();

                    TotalCount = ECatalogueData.Count();
                    ECatalogueData = await ApplyDtoSorting(ECatalogueData, Input);
                    ECatalogueData = ECatalogueData.Skip(Input.SkipCount).Take(Input.MaxResultCount).ToList();

                    if (Input.SkipCount + Input.MaxResultCount < TotalCount)
                    {
                        IsLoadMore = true;
                    }
                    customECatalogue.SkipCount = Input.SkipCount;
                    customECatalogue.items = ObjectMapper.Map<List<ECatalogueDto>>(ECatalogueData);
                    customECatalogue.TotalCount = TotalCount;
                    customECatalogue.IsLoadMore = IsLoadMore;
                }
            }
            catch (Exception ex)
            {

            }
            return customECatalogue;
        }

        private async Task<List<ECatalogueDto>> ApplyDtoSorting(List<ECatalogueDto> query, ECatalogueRequestDto input)
        {
            if (input.Sorting.HasValue)
            {
                switch (input.Sorting)
                {
                    case 1:
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.Id).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Id).ToList();
                        }

                        break;
                    case 2://
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.Title).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Title).ToList();
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.Title).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Title).ToList();
                        }
                        break;
                    default:
                        {
                            query = query.OrderBy(x => x.Title).ToList();
                        }
                        break;
                }

            }
            else
            {
                // query = query.OrderBy(x => x.CategoryTitle).ToList();
                query = query.OrderByDescending(x => x.Id).ToList();
            }
            return query;
        }
    }
}
