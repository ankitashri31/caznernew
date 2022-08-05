using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}