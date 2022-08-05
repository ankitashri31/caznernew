using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
