using System.Collections.Generic;

namespace CaznerMarketplaceBackendApp.MultiTenancy.Payments
{
    public interface IPaymentGatewayStore
    {
        List<PaymentGatewayModel> GetActiveGateways();
    }
}
