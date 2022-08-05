using Abp.AspNetZeroCore;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using CaznerMarketplaceBackendApp.Configuration;
using CaznerMarketplaceBackendApp.EntityFrameworkCore;
using CaznerMarketplaceBackendApp.Migrator.DependencyInjection;

namespace CaznerMarketplaceBackendApp.Migrator
{
    [DependsOn(typeof(CaznerMarketplaceBackendAppEntityFrameworkCoreModule))]
    public class CaznerMarketplaceBackendAppMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public CaznerMarketplaceBackendAppMigratorModule(CaznerMarketplaceBackendAppEntityFrameworkCoreModule abpZeroTemplateEntityFrameworkCoreModule)
        {
            abpZeroTemplateEntityFrameworkCoreModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(CaznerMarketplaceBackendAppMigratorModule).GetAssembly().GetDirectoryPathOrNull(),
                addUserSecrets: true
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                CaznerMarketplaceBackendAppConsts.ConnectionStringName
                );
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CaznerMarketplaceBackendAppMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}