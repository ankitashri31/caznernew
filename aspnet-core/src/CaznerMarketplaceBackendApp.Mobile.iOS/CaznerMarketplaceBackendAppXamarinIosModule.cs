using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CaznerMarketplaceBackendApp
{
    [DependsOn(typeof(CaznerMarketplaceBackendAppXamarinSharedModule))]
    public class CaznerMarketplaceBackendAppXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CaznerMarketplaceBackendAppXamarinIosModule).GetAssembly());
        }
    }
}