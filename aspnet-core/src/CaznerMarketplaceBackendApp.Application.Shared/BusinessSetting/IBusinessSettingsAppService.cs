using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Authorization.Users.Dto;
using CaznerMarketplaceBackendApp.BusinessSetting.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.BusinessSetting
{
    public interface IBusinessSettingsAppService : IApplicationService
    {
        Task<UserBusinessSettingsDto> CreateUserSettings(UserBusinessSettingsDto userBusinessSettingsDto);
        Task<UserShippingAddressDto> GetUserPrimaryAddress();
        Task<List<UserShippingAddressDto>> GetUserAllShippingAddress();
    }
}
