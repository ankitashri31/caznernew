using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.EquipmentsBranding.Dto
{
    public class CreateOrUpdateEquipmentBranding
    {
        public string EquipmentTitle { get; set; }
        public bool IsActive { get; set; }
    }
}
