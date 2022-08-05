using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CaznerMarketplaceBackendApp.Startup
{
    [DependsOn(typeof(CaznerMarketplaceBackendAppCoreModule))]
    public class CaznerMarketplaceBackendAppGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CaznerMarketplaceBackendAppGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}