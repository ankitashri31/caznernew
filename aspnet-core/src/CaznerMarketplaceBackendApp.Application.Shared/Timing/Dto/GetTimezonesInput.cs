using Abp.Configuration;

namespace CaznerMarketplaceBackendApp.Timing.Dto
{
    public class GetTimezonesInput
    {
        public SettingScopes DefaultTimezoneScope { get; set; }
    }
}
