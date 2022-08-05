using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.Authorization.Permissions.Dto;

namespace CaznerMarketplaceBackendApp.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
