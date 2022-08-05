using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.WebHooks.Dto;

namespace CaznerMarketplaceBackendApp.WebHooks
{
    public interface IWebhookAttemptAppService
    {
        Task<PagedResultDto<GetAllSendAttemptsOutput>> GetAllSendAttempts(GetAllSendAttemptsInput input);

        Task<ListResultDto<GetAllSendAttemptsOfWebhookEventOutput>> GetAllSendAttemptsOfWebhookEvent(GetAllSendAttemptsOfWebhookEventInput input);
    }
}
