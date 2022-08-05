using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.MultiTenancy.Payments
{
    public class SubscriptionPlanMaster : FullAuditedEntity<long>
    {
        public string PlanTitle { get; set; }
        public string ShortDescription { get; set; }
        public double Amount { get; set; }
        public int CurrencyType { get; set; }
        public int CurrencySymbol { get; set; }
        public int UserType { get; set; }
        public int BillingTypeFrequency { get; set; }
        public bool IsMostPopularPlan { get; set; }
    }
}
