using Abp.Application.Services;
using CaznerMarketplaceBackendApp.BannerLogo.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BannerLogo
{
    public interface IImageTypeMasterAppService: IAsyncCrudAppService<ImageTypeMasterDto, long, ImageTypeMasterResultRequestDto, CreateOrUpdateImageTypeMaster, ImageTypeMasterDto>
    {
    }
}
