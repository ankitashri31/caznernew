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
using Abp.Authorization;

namespace CaznerMarketplaceBackendApp.Artwork
{
    public class ArtworkFrontendAppService : CaznerMarketplaceBackendAppAppServiceBase, IArtworkFrontendAppService
    {
        private readonly IRepository<ArtWorkMaster, long> _masterRepository;
        private IConfiguration _configuration;
        private readonly IRepository<Tenant, int> _tenantManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public ArtworkFrontendAppService(
            IRepository<ArtWorkMaster, long> masterRepository,
            IRepository<Tenant, int> tenantManager, IUnitOfWorkManager unitOfWorkManager, IConfiguration configuration
            )
        {
            _masterRepository = masterRepository;
         
            _tenantManager = tenantManager;
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = configuration;
         
        }

        public async Task<ArtworkFronEndDto> GetArtWorkByTenantId(ArtworkDataRequestDto Input)
        {
            ArtworkFronEndDto response = new ArtworkFronEndDto();
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
                var artWorkData = _masterRepository.GetAllList(i => i.TenantId == Convert.ToInt32(TenantId)).FirstOrDefault();
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
