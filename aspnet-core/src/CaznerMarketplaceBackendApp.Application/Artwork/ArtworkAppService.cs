using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using CaznerMarketplaceBackendApp.Artwork.Dto;
using CaznerMarketplaceBackendApp.ArtWork;
using CaznerMarketplaceBackendApp.MultiTenancy;
using FimApp.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.Artwork
{
    public class ArtworkAppService : CaznerMarketplaceBackendAppAppServiceBase, IArtworkAppService
    {

        private readonly IRepository<ArtWorkMaster, long> _masterRepository;
        private readonly IRepository<ArtworkImages, long> _ImageRepository;
        private readonly IRepository<ArtworkMockupImages, long> _MockUpImageRepository;
        string AzureStorageUrl = string.Empty;
        private IConfiguration _configuration;
        private readonly IRepository<Tenant, int> _tenantManager;
        string AzureProductFolder = string.Empty;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public ArtworkAppService(
            IRepository<ArtWorkMaster, long> masterRepository,
            IRepository<ArtworkImages, long> ImageRepository,
            IRepository<ArtworkMockupImages, long> MockUpImageRepository,
            IRepository<Tenant, int> tenantManager, IUnitOfWorkManager unitOfWorkManager, IConfiguration configuration
            )
        {
            _masterRepository = masterRepository;
            _ImageRepository = ImageRepository;
            _MockUpImageRepository = MockUpImageRepository;
            _tenantManager = tenantManager;
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = configuration;
            AzureProductFolder = _configuration["FileUpload:AzureProductFolder"];
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];

        }
        public async Task CreateArtWork(ArtworkDto createArtWork)
        {
            try
            {
                if (createArtWork.IsArtwork == true)
                {
                    int TenantId = AbpSession.TenantId.Value;
                    var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
                    string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
                    var ArtworkIsExists = _masterRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value).FirstOrDefault();
                    if (ArtworkIsExists == null)
                    {
                        var artWork = _masterRepository.GetAllList(x => x.ArtworkSKU == createArtWork.ArtworkSKU).FirstOrDefault();
                        if (artWork == null)
                        {
                            var ArtworkData = ObjectMapper.Map<ArtWorkMaster>(createArtWork);
                            ArtworkData.TenantId = AbpSession.TenantId.Value;
                            ArtworkData.ArtworkUniqueId = Guid.NewGuid().ToString();
                            long ArtworkId = await _masterRepository.InsertAndGetIdAsync(ArtworkData);
                        }
                    }
                    else
                    {

                        ArtworkIsExists.ApprovalDescription = createArtWork.ApprovalDescription;
                        ArtworkIsExists.ArtworkDescription = createArtWork.ArtworkDescription;
                        ArtworkIsExists.ArtworkFeeTitle = createArtWork.ArtworkFeeTitle;
                        ArtworkIsExists.ArtworkNote = createArtWork.ArtworkNote;
                        ArtworkIsExists.ArtworkSKU = createArtWork.ArtworkSKU;
                        ArtworkIsExists.HandlingCharge = createArtWork.HandlingCharge;
                        ArtworkIsExists.UnitPrice = createArtWork.UnitPrice;
                        ArtworkIsExists.IsArtworkEnabled = createArtWork.IsArtworkEnabled;
                        ArtworkIsExists.IsEnableForMockups = createArtWork.IsEnableForMockups;
                        await _masterRepository.UpdateAsync(ArtworkIsExists);

                    }

                }
                else
                {
                    int TenantId = AbpSession.TenantId.Value;
                    var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
                    string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
                    var ArtworkIsExists = _masterRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value).FirstOrDefault();
                    if (ArtworkIsExists == null)
                    {
                        var artWork = _masterRepository.GetAllList(x => x.MockupSKU == createArtWork.MockupSKU).FirstOrDefault();
                        if (artWork == null)
                        {
                            var ArtworkData = ObjectMapper.Map<ArtWorkMaster>(createArtWork);
                            ArtworkData.TenantId = AbpSession.TenantId.Value;
                            ArtworkData.MockupUniqueId = Guid.NewGuid().ToString();
                            long ArtworkId = await _masterRepository.InsertAndGetIdAsync(ArtworkData);
                        }
                    }
                    else
                    {
                        ArtworkIsExists.MaxNumberOfMockUpCanOrder = createArtWork.MaxNumberOfMockUpCanOrder.ToString() == "" ? 0 : Convert.ToDouble(createArtWork.MaxNumberOfMockUpCanOrder);
                        ArtworkIsExists.MockupDescription = createArtWork.MockupDescription;
                        ArtworkIsExists.MockupPrice = !string.IsNullOrEmpty(createArtWork.MockupPrice.ToString()) ? Convert.ToDecimal(createArtWork.MockupPrice) : 0;
                        ArtworkIsExists.MockupSKU = createArtWork.MockupSKU;
                        ArtworkIsExists.MockupTitle = createArtWork.MockupTitle;
                        ArtworkIsExists.UnitPrice = createArtWork.UnitPrice;
                        ArtworkIsExists.HandlingCharge = createArtWork.HandlingCharge;
                        ArtworkIsExists.IsEnableForMockups = createArtWork.IsEnableForMockups;
                        await _masterRepository.UpdateAsync(ArtworkIsExists);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<ArtworkDto> GetArtWorkByTenantId()
        {
            ArtworkDto response = new ArtworkDto();
            int TenantId = AbpSession.TenantId.Value;
            try
            {
                var artWorkData = _masterRepository.GetAllList(i => i.TenantId == TenantId).FirstOrDefault();
                if (artWorkData != null)
                {
                    response.Id = artWorkData.Id;
                    response.ArtworkUniqueId = artWorkData.ArtworkUniqueId;
                    response.ArtworkSKU = artWorkData.ArtworkSKU;
                    response.ArtworkFeeTitle = artWorkData.ArtworkFeeTitle;
                    response.ArtworkDescription = artWorkData.ArtworkDescription;
                    response.UnitPrice = artWorkData.UnitPrice;
                    response.HandlingCharge = artWorkData.HandlingCharge;
                    response.ApprovalDescription = artWorkData.ApprovalDescription;
                    response.ArtworkNote = artWorkData.ArtworkNote;
                    response.IsEnableForMockups = artWorkData.IsEnableForMockups;
                    response.IsArtworkEnabled = artWorkData.IsArtworkEnabled;
                    response.MockupSKU = artWorkData.MockupSKU;
                    response.MockupUniqueId = artWorkData.MockupUniqueId;
                    response.MockupTitle = artWorkData.MockupTitle;
                    response.MockupDescription = artWorkData.MockupDescription;
                    response.MockupPrice = artWorkData.MockupPrice;
                    response.MaxNumberOfMockUpCanOrder = artWorkData.MaxNumberOfMockUpCanOrder;

                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return response;


        }

    }
}
