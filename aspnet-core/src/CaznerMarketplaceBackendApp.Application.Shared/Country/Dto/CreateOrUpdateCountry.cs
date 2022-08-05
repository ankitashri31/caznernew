using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Country.Dto
{
  
    public class CreateOrUpdateCountry
    {
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
    }
}
