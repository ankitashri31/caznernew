using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace CaznerMarketplaceBackendApp.Web.Public.Views
{
    public abstract class CaznerMarketplaceBackendAppRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected CaznerMarketplaceBackendAppRazorPage()
        {
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
        }
    }
}
