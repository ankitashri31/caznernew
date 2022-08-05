using System.Threading.Tasks;
using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Configuration.Host.Dto;

namespace CaznerMarketplaceBackendApp.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
