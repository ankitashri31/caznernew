using System;

namespace CaznerMarketplaceBackendApp.WebHooks.Dto
{
    public class ActivateWebhookSubscriptionInput
    {
        public Guid SubscriptionId { get; set; }

        public bool IsActive { get; set; }
    }
}
