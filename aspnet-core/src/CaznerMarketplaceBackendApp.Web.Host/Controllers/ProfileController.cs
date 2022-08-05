using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using CaznerMarketplaceBackendApp.Authorization.Users.Profile;
using CaznerMarketplaceBackendApp.Storage;

namespace CaznerMarketplaceBackendApp.Web.Controllers
{
    [Authorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService) :
            base(tempFileCacheManager, profileAppService)
        {
        }
    }
}