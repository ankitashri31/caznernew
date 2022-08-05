using System.Threading.Tasks;
using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Sessions.Dto;

namespace CaznerMarketplaceBackendApp.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
