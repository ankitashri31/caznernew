using System.Threading.Tasks;
using Abp.Application.Services;
using CaznerMarketplaceBackendApp.MultiTenancy.Payments.Dto;
using CaznerMarketplaceBackendApp.MultiTenancy.Payments.Stripe.Dto;

namespace CaznerMarketplaceBackendApp.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}