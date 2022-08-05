using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CaznerMarketplaceBackendApp
{
    public class CaznerMarketplaceBackendAppCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CaznerMarketplaceBackendAppCoreSharedModule).GetAssembly());
        }
    }
}