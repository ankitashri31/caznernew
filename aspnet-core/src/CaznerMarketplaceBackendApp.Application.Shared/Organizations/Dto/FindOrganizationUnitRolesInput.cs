using CaznerMarketplaceBackendApp.Dto;

namespace CaznerMarketplaceBackendApp.Organizations.Dto
{
    public class FindOrganizationUnitRolesInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}