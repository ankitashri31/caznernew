using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CaznerMarketplaceBackendApp
{
    [DependsOn(typeof(CaznerMarketplaceBackendAppCoreSharedModule))]
    public class CaznerMarketplaceBackendAppApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CaznerMarketplaceBackendAppApplicationSharedModule).GetAssembly());
        }
    }
}