using CaznerMarketplaceBackendApp.Dto;

namespace CaznerMarketplaceBackendApp.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }

        public bool ExcludeCurrentUser { get; set; }
    }
}