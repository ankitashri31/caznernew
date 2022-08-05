using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.Common.Dto;
using CaznerMarketplaceBackendApp.Editions.Dto;

namespace CaznerMarketplaceBackendApp.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}