using Abp.Authorization;
using CaznerMarketplaceBackendApp.Authorization.Roles;
using CaznerMarketplaceBackendApp.Authorization.Users;

namespace CaznerMarketplaceBackendApp.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
