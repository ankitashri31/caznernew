using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using CaznerMarketplaceBackendApp.Configure;
using CaznerMarketplaceBackendApp.Startup;
using CaznerMarketplaceBackendApp.Test.Base;

namespace CaznerMarketplaceBackendApp.GraphQL.Tests
{
    [DependsOn(
        typeof(CaznerMarketplaceBackendAppGraphQLModule),
        typeof(CaznerMarketplaceBackendAppTestBaseModule))]
    public class CaznerMarketplaceBackendAppGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CaznerMarketplaceBackendAppGraphQLTestModule).GetAssembly());
        }
    }
}