using Abp.Dependency;
using CaznerMarketplaceBackendApp.Configuration;
using CaznerMarketplaceBackendApp.Url;

namespace CaznerMarketplaceBackendApp.Web.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor configurationAccessor) :
            base(configurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:ClientRootAddress";

        public override string ServerRootAddressFormatKey => "App:ServerRootAddress";
    }
}