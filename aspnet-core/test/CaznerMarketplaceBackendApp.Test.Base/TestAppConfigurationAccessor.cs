using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using CaznerMarketplaceBackendApp.Configuration;

namespace CaznerMarketplaceBackendApp.Test.Base
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(CaznerMarketplaceBackendAppTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
