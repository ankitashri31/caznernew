using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.MultiTenancy.Accounting.Dto;

namespace CaznerMarketplaceBackendApp.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
