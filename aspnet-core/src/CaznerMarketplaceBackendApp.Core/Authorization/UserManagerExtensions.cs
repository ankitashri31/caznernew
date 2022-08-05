using System.Threading.Tasks;
using Abp.Authorization.Users;
using CaznerMarketplaceBackendApp.Authorization.Users;

namespace CaznerMarketplaceBackendApp.Authorization
{
    public static class UserManagerExtensions
    {
        public static async Task<User> GetAdminAsync(this UserManager userManager)
        {
            return await userManager.FindByNameAsync(AbpUserBase.AdminUserName);
        }
    }
}
