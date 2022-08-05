using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.MultiTenancy.HostDashboard.Dto;

namespace CaznerMarketplaceBackendApp.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}