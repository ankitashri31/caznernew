using System.Collections.Generic;
using MvvmHelpers;
using CaznerMarketplaceBackendApp.Models.NavigationMenu;

namespace CaznerMarketplaceBackendApp.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}