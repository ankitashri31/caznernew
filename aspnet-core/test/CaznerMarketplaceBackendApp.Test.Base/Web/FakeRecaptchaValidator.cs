using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.Security.Recaptcha;

namespace CaznerMarketplaceBackendApp.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
