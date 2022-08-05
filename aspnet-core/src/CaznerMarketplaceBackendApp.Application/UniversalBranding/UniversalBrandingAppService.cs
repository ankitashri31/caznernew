using Abp.Application.Services;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.BrandingMethods;
using CaznerMarketplaceBackendApp.UniversalBranding.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.UniversalBranding
{
    public class UniversalBrandingAppService :
        AsyncCrudAppService<UniversalBrandingMaster, UniversalBrandingMasterDto, long, UniversalBrandingResultRequestDto, CreateOrUpdateUniversalBranding, UniversalBrandingMasterDto>, IUniversalBrandingAppService
    {
        public UniversalBrandingAppService(IRepository<UniversalBrandingMaster, long> repository) : base(repository)
        {
        }
    }
}
