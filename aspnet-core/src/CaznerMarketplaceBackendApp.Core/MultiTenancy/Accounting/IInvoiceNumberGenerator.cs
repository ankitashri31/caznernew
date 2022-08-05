using System.Threading.Tasks;
using Abp.Dependency;

namespace CaznerMarketplaceBackendApp.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}