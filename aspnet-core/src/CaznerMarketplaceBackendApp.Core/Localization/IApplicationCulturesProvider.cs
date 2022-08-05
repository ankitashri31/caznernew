using System.Globalization;

namespace CaznerMarketplaceBackendApp.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}