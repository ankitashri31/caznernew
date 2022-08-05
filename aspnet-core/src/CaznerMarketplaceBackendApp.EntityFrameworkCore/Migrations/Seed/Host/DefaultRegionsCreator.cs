using System;
using Abp.MultiTenancy;
using CaznerMarketplaceBackendApp.Country;
using CaznerMarketplaceBackendApp.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CaznerMarketplaceBackendApp.Migrations.Seed.Host
{
    public class DefaultRegionsCreator
    {
        public static List<Regions> InitialRegions => GetInitialRegions();

        private readonly CaznerMarketplaceBackendAppDbContext _context;

        private static List<Regions> GetInitialRegions()
        {
            var tenantId = CaznerMarketplaceBackendAppConsts.MultiTenancyEnabled ? null : (int?)MultiTenancyConsts.DefaultTenantId;
            return new List<Regions>
            {
                new Regions("African Region","AFR"),
                new Regions("Western Pacific Region", "WPR"),
                new Regions("South-East Asia Region", "SEAR"),
                new Regions("European Region", "EUR"),
                new Regions("Region of the Americas","AMR" ),
                new Regions("Eastern Mediterranean Region","EMR"),

            };
        }
        public DefaultRegionsCreator(CaznerMarketplaceBackendAppDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateRegions();
        }

        private void CreateRegions()
        {
            foreach (var region in InitialRegions)
            {
                AddRegionsIfNotExists(region);
            }
        }

        private void AddRegionsIfNotExists(Regions region)
        {
            if (_context.Regions.IgnoreQueryFilters().Any(l => l.RegionCode == region.RegionCode && l.Title == region.Title))
            {
                return;
            }

            _context.Regions.Add(region);
            _context.SaveChanges();
        }
    }

}