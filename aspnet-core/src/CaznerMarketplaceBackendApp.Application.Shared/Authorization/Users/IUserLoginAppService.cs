using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.Authorization.Users.Dto;

namespace CaznerMarketplaceBackendApp.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
