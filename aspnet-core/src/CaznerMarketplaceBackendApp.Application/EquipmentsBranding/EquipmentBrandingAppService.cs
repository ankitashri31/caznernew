using Abp.Application.Services;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.BrandingMethods;
using CaznerMarketplaceBackendApp.EquipmentsBranding.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.EquipmentsBranding
{
    public class EquipmentBrandingAppService:
         AsyncCrudAppService<EquipmentBrandingMaster, EquipmentBrandingMasterDto, long, EquipmentBrandingResultRequestDto, CreateOrUpdateEquipmentBranding, EquipmentBrandingMasterDto>, IEquipmentBrandingAppService
    {
        public EquipmentBrandingAppService(IRepository<EquipmentBrandingMaster, long> repository) : base(repository)
        {
        }
    }
}
