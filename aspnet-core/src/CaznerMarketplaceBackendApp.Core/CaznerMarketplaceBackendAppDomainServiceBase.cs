using Abp.Domain.Services;

namespace CaznerMarketplaceBackendApp
{
    public abstract class CaznerMarketplaceBackendAppDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected CaznerMarketplaceBackendAppDomainServiceBase()
        {
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
        }
    }
}
