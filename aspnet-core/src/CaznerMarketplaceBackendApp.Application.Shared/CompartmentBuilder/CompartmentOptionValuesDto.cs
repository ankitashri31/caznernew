using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CompartmentBuilder
{
    public class CompartmentOptionValuesDto
    {
        public int TenantId { get; set; }
        public long CompartmentVariantId { get; set; }
        public long OptionId { get; set; }
        public string CompartmentOptionValue { get; set; }
        public bool IsActive { get; set; }
    }
}
