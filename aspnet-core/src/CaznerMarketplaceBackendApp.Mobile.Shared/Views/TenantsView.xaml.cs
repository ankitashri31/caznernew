using CaznerMarketplaceBackendApp.Models.Tenants;
using CaznerMarketplaceBackendApp.ViewModels;
using Xamarin.Forms;

namespace CaznerMarketplaceBackendApp.Views
{
    public partial class TenantsView : ContentPage, IXamarinView
    {
        public TenantsView()
        {
            InitializeComponent();
        }

        private async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((TenantsViewModel)BindingContext).LoadMoreTenantsIfNeedsAsync(e.Item as TenantListModel);
        }
    }
}