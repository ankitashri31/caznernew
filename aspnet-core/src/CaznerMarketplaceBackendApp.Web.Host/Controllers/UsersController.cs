using Abp.AspNetCore.Mvc.Authorization;
using CaznerMarketplaceBackendApp.Authorization;
using CaznerMarketplaceBackendApp.Storage;
using Abp.BackgroundJobs;

namespace CaznerMarketplaceBackendApp.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}