using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Position.Dto
{
    public class CreateOrUpdatePosition
    {
        public string PositionName { get; set; }        
        public bool IsActive { get; set; }
    }
}
