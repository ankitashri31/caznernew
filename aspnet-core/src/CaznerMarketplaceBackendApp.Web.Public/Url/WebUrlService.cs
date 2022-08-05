using Abp.Dependency;
using CaznerMarketplaceBackendApp.Configuration;
using CaznerMarketplaceBackendApp.Url;
using CaznerMarketplaceBackendApp.Web.Url;

namespace CaznerMarketplaceBackendApp.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}