using System.Threading.Tasks;
using Abp.Application.Services;
using CaznerMarketplaceBackendApp.MultiTenancy.Payments.PayPal.Dto;

namespace CaznerMarketplaceBackendApp.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
