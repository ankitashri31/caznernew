using System.Threading.Tasks;
using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Editions.Dto;
using CaznerMarketplaceBackendApp.MultiTenancy.Dto;

namespace CaznerMarketplaceBackendApp.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}