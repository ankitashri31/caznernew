using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.UniversalBranding.Dto
{
    public class CreateOrUpdateUniversalBranding
    {
        public string UniversalBrandingTitle { get; set; }
        public bool IsActive { get; set; }
    }
}
