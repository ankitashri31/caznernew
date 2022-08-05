using Abp.AspNetCore.Mvc.ViewComponents;

namespace CaznerMarketplaceBackendApp.Web.Public.Views
{
    public abstract class CaznerMarketplaceBackendAppViewComponent : AbpViewComponent
    {
        protected CaznerMarketplaceBackendAppViewComponent()
        {
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
        }
    }
}