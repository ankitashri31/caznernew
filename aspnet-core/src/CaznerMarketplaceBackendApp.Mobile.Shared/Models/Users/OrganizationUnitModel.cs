using Abp.AutoMapper;
using CaznerMarketplaceBackendApp.Organizations.Dto;

namespace CaznerMarketplaceBackendApp.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}