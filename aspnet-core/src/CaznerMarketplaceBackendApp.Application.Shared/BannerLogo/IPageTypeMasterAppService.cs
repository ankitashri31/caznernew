using Abp.Application.Services;
using CaznerMarketplaceBackendApp.BannerLogo.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BannerLogo
{
    public interface IPageTypeMasterAppService : IAsyncCrudAppService<BannerPageTypeMasterDto, long, BannerLogoResultRequestDto, CreateOrUpdateBannerLogo, BannerPageTypeMasterDto>
    {
    }
}
