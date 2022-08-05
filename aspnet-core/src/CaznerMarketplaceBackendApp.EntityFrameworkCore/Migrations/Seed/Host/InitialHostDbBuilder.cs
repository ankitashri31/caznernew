using CaznerMarketplaceBackendApp.EntityFrameworkCore;

namespace CaznerMarketplaceBackendApp.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly CaznerMarketplaceBackendAppDbContext _context;

        public InitialHostDbBuilder(CaznerMarketplaceBackendAppDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new DefaultRegionsCreator(_context).Create();
            new DefaultCountryCreator(_context).Create();
            new DefaultCurrencyCreator(_context).Create();
           
            _context.SaveChanges();
        }
    }
}
