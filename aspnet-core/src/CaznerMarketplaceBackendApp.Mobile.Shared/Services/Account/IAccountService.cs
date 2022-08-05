using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.ApiClient.Models;

namespace CaznerMarketplaceBackendApp.Services.Account
{
    public interface IAccountService
    {
        AbpAuthenticateModel AbpAuthenticateModel { get; set; }
        
        AbpAuthenticateResultModel AuthenticateResultModel { get; set; }
        
        Task LoginUserAsync();

        Task LogoutAsync();
    }
}
