﻿using Abp.Extensions;

namespace CaznerMarketplaceBackendApp.Authentication
{
    public class MicrosoftExternalLoginProviderSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public bool IsValid()
        {
            return !ClientId.IsNullOrWhiteSpace() && !ClientSecret.IsNullOrWhiteSpace();
        }
    }
}
