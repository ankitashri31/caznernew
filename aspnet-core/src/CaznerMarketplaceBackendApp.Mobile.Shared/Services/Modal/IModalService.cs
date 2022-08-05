using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.Views;
using Xamarin.Forms;

namespace CaznerMarketplaceBackendApp.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
