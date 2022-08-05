using Xamarin.Forms.Internals;

namespace CaznerMarketplaceBackendApp.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}