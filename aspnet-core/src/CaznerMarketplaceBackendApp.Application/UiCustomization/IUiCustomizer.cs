using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using CaznerMarketplaceBackendApp.Configuration.Dto;
using CaznerMarketplaceBackendApp.UiCustomization.Dto;

namespace CaznerMarketplaceBackendApp.UiCustomization
{
    public interface IUiCustomizer : ISingletonDependency
    {
        Task<UiCustomizationSettingsDto> GetUiSettings();

        Task UpdateUserUiManagementSettingsAsync(UserIdentifier user, ThemeSettingsDto settings);

        Task UpdateTenantUiManagementSettingsAsync(int tenantId, ThemeSettingsDto settings);

        Task UpdateApplicationUiManagementSettingsAsync(ThemeSettingsDto settings);

        Task<ThemeSettingsDto> GetHostUiManagementSettings();

        Task<ThemeSettingsDto> GetTenantUiCustomizationSettings(int tenantId);
    }
}
