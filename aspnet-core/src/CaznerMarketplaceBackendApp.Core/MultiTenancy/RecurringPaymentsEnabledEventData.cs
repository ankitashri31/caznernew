using Abp.Events.Bus;

namespace CaznerMarketplaceBackendApp.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}