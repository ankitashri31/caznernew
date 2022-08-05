using Microsoft.Extensions.DependencyInjection;
using CaznerMarketplaceBackendApp.HealthChecks;

namespace CaznerMarketplaceBackendApp.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<CaznerMarketplaceBackendAppDbContextHealthCheck>("Database Connection");
            builder.AddCheck<CaznerMarketplaceBackendAppDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
