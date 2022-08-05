using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace CaznerMarketplaceBackendApp.Localization
{
    public static class CaznerMarketplaceBackendAppLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    CaznerMarketplaceBackendAppConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(CaznerMarketplaceBackendAppLocalizationConfigurer).GetAssembly(),
                        "CaznerMarketplaceBackendApp.Localization.CaznerMarketplaceBackendApp"
                    )
                )
            );
        }
    }
}