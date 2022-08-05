using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CaznerMarketplaceBackendApp.Country
{
    public class StatesTempRawData
    {
        [Key]
        public long Id { get; set; }
        public string CountryISOCode { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }

    }
}
