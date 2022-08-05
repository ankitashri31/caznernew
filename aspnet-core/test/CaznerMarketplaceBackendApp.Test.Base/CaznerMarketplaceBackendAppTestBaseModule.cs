using System;
using System.IO;
using Abp;
using Abp.AspNetZeroCore;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Configuration;
using CaznerMarketplaceBackendApp.EntityFrameworkCore;
using CaznerMarketplaceBackendApp.MultiTenancy;
using CaznerMarketplaceBackendApp.Security.Recaptcha;
using CaznerMarketplaceBackendApp.Test.Base.DependencyInjection;
using CaznerMarketplaceBackendApp.Test.Base.UiCustomization;
using CaznerMarketplaceBackendApp.Test.Base.Url;
using CaznerMarketplaceBackendApp.Test.Base.Web;
using CaznerMarketplaceBackendApp.UiCustomization;
using CaznerMarketplaceBackendApp.Url;
using NSubstitute;

namespace CaznerMarketplaceBackendApp.Test.Base
{
    [DependsOn(
        typeof(CaznerMarketplaceBackendAppApplicationModule),
        typeof(CaznerMarketplaceBackendAppEntityFrameworkCoreModule),
        typeof(AbpTestBaseModule))]
    public class CaznerMarketplaceBackendAppTestBaseModule : AbpModule
    {
        public CaznerMarketplaceBackendAppTestBaseModule(CaznerMarketplaceBackendAppEntityFrameworkCoreModule abpZeroTemplateEntityFrameworkCoreModule)
        {
            abpZeroTemplateEntityFrameworkCoreModule.SkipDbContextRegistration = true;
        }

        public override void PreInitialize()
        {
            var configuration = GetConfiguration();

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;

            //Disable static mapper usage since it breaks unit tests (see https://github.com/aspnetboilerplate/aspnetboilerplate/issues/2052)
            Configuration.Modules.AbpAutoMapper().UseStaticMapper = false;

            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            RegisterFakeService<AbpZeroDbMigrator>();

            IocManager.Register<IAppUrlService, FakeAppUrlService>();
            IocManager.Register<IWebUrlService, FakeWebUrlService>();
            IocManager.Register<IRecaptchaValidator, FakeRecaptchaValidator>();

            Configuration.ReplaceService<IAppConfigurationAccessor, TestAppConfigurationAccessor>();
            Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
            Configuration.ReplaceService<IUiThemeCustomizerFactory, NullUiThemeCustomizerFactory>();

            Configuration.Modules.AspNetZero().LicenseCode = configuration["AbpZeroLicenseCode"];

            //Uncomment below line to write change logs for the entities below:
            Configuration.EntityHistory.IsEnabled = true;
            Configuration.EntityHistory.Selectors.Add("CaznerMarketplaceBackendAppEntities", typeof(User), typeof(Tenant));
        }

        public override void Initialize()
        {
            ServiceCollectionRegistrar.Register(IocManager);
        }

        private void RegisterFakeService<TService>()
            where TService : class
        {
            IocManager.IocContainer.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(() => Substitute.For<TService>())
                    .LifestyleSingleton()
            );
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return AppConfigurations.Get(Directory.GetCurrentDirectory(), addUserSecrets: true);
        }
    }
}
