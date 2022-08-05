using System.Threading.Tasks;
using Abp.Webhooks;

namespace CaznerMarketplaceBackendApp.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
