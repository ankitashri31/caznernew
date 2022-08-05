using CaznerMarketplaceBackendApp.Models.Users;
using CaznerMarketplaceBackendApp.ViewModels;
using Xamarin.Forms;

namespace CaznerMarketplaceBackendApp.Views
{
    public partial class UsersView : ContentPage, IXamarinView
    {
        public UsersView()
        {
            InitializeComponent();
        }

        public async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((UsersViewModel) BindingContext).LoadMoreUserIfNeedsAsync(e.Item as UserListModel);
        }
    }
}