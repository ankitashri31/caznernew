using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.Sessions.Dto;

namespace CaznerMarketplaceBackendApp.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
