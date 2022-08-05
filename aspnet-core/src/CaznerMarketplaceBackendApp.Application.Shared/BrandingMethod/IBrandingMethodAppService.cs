using Abp.Application.Services;
using CaznerMarketplaceBackendApp.BrandingMethod.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.BrandingMethod
{
    public interface IBrandingMethodAppService : IAsyncCrudAppService<BrandingMethodDto, long, BrandingMethodResultRequestDto, CreateOrUpdateBrandingMethod, BrandingMethodDto>
    {
        Task<CreateBrandingMethodDto> CreateBrandingMethod(CreateBrandingMethodDto createBrandingMethoddto);
    }
}
