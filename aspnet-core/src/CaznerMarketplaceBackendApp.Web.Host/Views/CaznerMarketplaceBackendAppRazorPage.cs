using Abp.AspNetCore.Mvc.Views;

namespace CaznerMarketplaceBackendApp.Web.Views
{
    public abstract class CaznerMarketplaceBackendAppRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected CaznerMarketplaceBackendAppRazorPage()
        {
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
        }
    }
}
