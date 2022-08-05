using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.Authorization.Users;

namespace CaznerMarketplaceBackendApp.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
