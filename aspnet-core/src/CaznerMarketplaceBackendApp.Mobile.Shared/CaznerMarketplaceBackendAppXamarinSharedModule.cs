using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CaznerMarketplaceBackendApp
{
    [DependsOn(typeof(CaznerMarketplaceBackendAppClientModule), typeof(AbpAutoMapperModule))]
    public class CaznerMarketplaceBackendAppXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CaznerMarketplaceBackendAppXamarinSharedModule).GetAssembly());
        }
    }
}