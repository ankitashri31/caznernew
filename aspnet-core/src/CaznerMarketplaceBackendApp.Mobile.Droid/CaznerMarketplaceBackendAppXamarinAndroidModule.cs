using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CaznerMarketplaceBackendApp
{
    [DependsOn(typeof(CaznerMarketplaceBackendAppXamarinSharedModule))]
    public class CaznerMarketplaceBackendAppXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CaznerMarketplaceBackendAppXamarinAndroidModule).GetAssembly());
        }
    }
}