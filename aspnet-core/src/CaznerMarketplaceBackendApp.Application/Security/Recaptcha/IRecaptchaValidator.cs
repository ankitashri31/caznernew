using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}