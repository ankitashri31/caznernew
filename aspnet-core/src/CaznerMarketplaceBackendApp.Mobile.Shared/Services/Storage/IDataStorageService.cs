using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.ApiClient;
using CaznerMarketplaceBackendApp.ApiClient.Models;
using CaznerMarketplaceBackendApp.Sessions.Dto;

namespace CaznerMarketplaceBackendApp.Services.Storage
{
    public interface IDataStorageService
    {
        Task StoreAccessTokenAsync(string newAccessToken);

        Task StoreAuthenticateResultAsync(AbpAuthenticateResultModel authenticateResultModel);

        AbpAuthenticateResultModel RetrieveAuthenticateResult();

        TenantInformation RetrieveTenantInfo();

        GetCurrentLoginInformationsOutput RetrieveLoginInfo();

        void ClearSessionPersistance();

        Task StoreLoginInformationAsync(GetCurrentLoginInformationsOutput loginInfo);

        Task StoreTenantInfoAsync(TenantInformation tenantInfo);
    }
}
