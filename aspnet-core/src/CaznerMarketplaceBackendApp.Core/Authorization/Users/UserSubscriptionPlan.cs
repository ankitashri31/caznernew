using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.MultiTenancy.Payments;

namespace CaznerMarketplaceBackendApp.Authorization.Users
{
    public class UserSubscriptionPlan : FullAuditedEntity<long>
    {
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }

        public virtual SubscriptionPlanMaster Subscription { get; set; }
        [ForeignKey("Subscription")]
        public long SubscriptionPlanId { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }

        public DateTime? SubscriptionEndDate { get; set; }

        public bool IsActive { get; set; }

    }
}
