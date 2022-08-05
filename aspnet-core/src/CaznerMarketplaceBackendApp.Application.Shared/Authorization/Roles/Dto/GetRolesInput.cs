using System.Collections.Generic;

namespace CaznerMarketplaceBackendApp.Authorization.Roles.Dto
{
    public class GetRolesInput
    {
        public List<string> Permissions { get; set; }
    }
}
