using System.Threading.Tasks;
using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Install.Dto;

namespace CaznerMarketplaceBackendApp.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}