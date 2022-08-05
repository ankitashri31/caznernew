using Microsoft.Extensions.Configuration;

namespace CaznerMarketplaceBackendApp.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
