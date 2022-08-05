using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Country
{
    public class Countries : FullAuditedEntity<long>
    {
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
        public string ISO2Code { get; set; }
        public string ISO3Code { get; set; }
        public virtual Regions Regions { get; set; }
        [ForeignKey("Regions")]
        public long? RegionId { get; set; }
        public string WorldBankIncomeGroup { get; set; }
        
        public Countries()
        {

        }

        public Countries(string countryName,bool isActive , string iso2Code = null , string iso3Code = null, long? regionId = null, string worldBankIncomeGroup = null)
            : this()
        {
            CountryName = countryName;
            IsActive = isActive;
            ISO2Code = iso2Code;
            ISO3Code = iso3Code;           
            RegionId = regionId;
            WorldBankIncomeGroup = worldBankIncomeGroup;

        }
    }
}
