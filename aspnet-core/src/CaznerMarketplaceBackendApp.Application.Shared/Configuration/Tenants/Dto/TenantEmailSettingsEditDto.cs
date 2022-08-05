using Abp.Auditing;
using CaznerMarketplaceBackendApp.Configuration.Dto;

namespace CaznerMarketplaceBackendApp.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}