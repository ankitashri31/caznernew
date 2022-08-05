using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CaznerMarketplaceBackendApp
{
    public class CaznerMarketplaceBackendAppClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CaznerMarketplaceBackendAppClientModule).GetAssembly());
        }
    }
}
