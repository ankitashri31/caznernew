using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.MultiTenancy.Payments
{
    public class SubscriptionFeaturesMaster : FullAuditedEntity<long>
    {
        public string FeatureTitle { get; set; }
        public Guid FeatureUniqueId { get; set; }
        public bool IsActive  { get; set; }

    }
}
