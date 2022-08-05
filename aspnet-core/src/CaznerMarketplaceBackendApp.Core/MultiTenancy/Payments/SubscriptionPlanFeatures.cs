using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.MultiTenancy.Payments
{
    public class SubscriptionPlanFeatures : FullAuditedEntity<long>
    {
        public virtual SubscriptionPlanMaster Subscription { get; set; }
        [ForeignKey("Subscription")]
        public long SubscriptionPlanId { get; set; }

        public virtual SubscriptionFeaturesMaster SubscriptionFeaturesMaster { get; set; }
        [ForeignKey("SubscriptionFeaturesMaster")]
        public long SubscriptionFeatureId { get; set; }

        public bool? IsAccessAllowed { get; set; }

        public bool IsFreeTextExists { get; set; }

        public string FreeText { get; set; }

        public int AllowedQuantity { get; set; }

        public bool IsActive { get; set; }
    }
}
