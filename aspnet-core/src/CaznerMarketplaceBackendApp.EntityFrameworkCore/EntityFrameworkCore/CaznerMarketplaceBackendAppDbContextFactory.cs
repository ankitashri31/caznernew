using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using CaznerMarketplaceBackendApp.Configuration;
using CaznerMarketplaceBackendApp.Web;

namespace CaznerMarketplaceBackendApp.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class CaznerMarketplaceBackendAppDbContextFactory : IDesignTimeDbContextFactory<CaznerMarketplaceBackendAppDbContext>
    {
        public CaznerMarketplaceBackendAppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CaznerMarketplaceBackendAppDbContext>();
            var configuration = AppConfigurations.Get(
                WebContentDirectoryFinder.CalculateContentRootFolder(),
                addUserSecrets: true
            );

            CaznerMarketplaceBackendAppDbContextConfigurer.Configure(builder, configuration.GetConnectionString(CaznerMarketplaceBackendAppConsts.ConnectionStringName));

            return new CaznerMarketplaceBackendAppDbContext(builder.Options);
        }
    }
}