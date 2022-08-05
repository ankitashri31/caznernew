using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using CaznerMarketplaceBackendApp.EntityFrameworkCore;

namespace CaznerMarketplaceBackendApp.HealthChecks
{
    public class CaznerMarketplaceBackendAppDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public CaznerMarketplaceBackendAppDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("CaznerMarketplaceBackendAppDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("CaznerMarketplaceBackendAppDbContext could not connect to database"));
        }
    }
}
