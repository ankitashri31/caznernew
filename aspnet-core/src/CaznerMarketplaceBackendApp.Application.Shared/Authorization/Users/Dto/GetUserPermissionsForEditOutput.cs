using System.Collections.Generic;
using CaznerMarketplaceBackendApp.Authorization.Permissions.Dto;

namespace CaznerMarketplaceBackendApp.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}