using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.SubscriptionFeatures.Dto
{
    public class CreateOrUpdateSubscriptionFeatures
    {
        public string FeatureTitle { get; set; }
        public Guid FeatureUniqueId { get; set; }
        public bool IsActive { get; set; }
    }
}
