using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.SubscriptionPlanFeature.Dto
{
    public class CreateOrUpdateSubscriptionPlanFeature
    {
        public long SubscriptionPlanId { get; set; }
        public long SubscriptionFeatureId { get; set; }

        public bool? IsAccessAllowed { get; set; }

        public bool IsFreeTextExists { get; set; }

        public string FreeText { get; set; }

        public int AllowedQuantity { get; set; }

        public bool IsActive { get; set; }
    }
}
