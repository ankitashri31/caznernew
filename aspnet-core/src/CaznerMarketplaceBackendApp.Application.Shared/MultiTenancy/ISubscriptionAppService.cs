using System.Threading.Tasks;
using Abp.Application.Services;

namespace CaznerMarketplaceBackendApp.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
