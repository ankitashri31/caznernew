using CaznerMarketplaceBackendApp.Dto;

namespace CaznerMarketplaceBackendApp.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
