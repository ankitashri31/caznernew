using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CaznerMarketplaceBackendApp.EntityFrameworkCore
{
    public static class CaznerMarketplaceBackendAppDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<CaznerMarketplaceBackendAppDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<CaznerMarketplaceBackendAppDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}