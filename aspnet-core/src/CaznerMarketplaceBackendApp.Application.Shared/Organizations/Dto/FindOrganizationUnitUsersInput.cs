using CaznerMarketplaceBackendApp.Dto;

namespace CaznerMarketplaceBackendApp.Organizations.Dto
{
    public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}
