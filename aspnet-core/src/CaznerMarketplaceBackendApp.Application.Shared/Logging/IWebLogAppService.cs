using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Dto;
using CaznerMarketplaceBackendApp.Logging.Dto;

namespace CaznerMarketplaceBackendApp.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
