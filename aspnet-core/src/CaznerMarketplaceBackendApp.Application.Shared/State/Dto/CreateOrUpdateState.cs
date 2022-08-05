using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.State.Dto
{
    public class CreateOrUpdateState
    {
        public string StateName { get; set; }
        public long CountryId { get; set; }
        public bool IsActive { get; set; }
    }
}
