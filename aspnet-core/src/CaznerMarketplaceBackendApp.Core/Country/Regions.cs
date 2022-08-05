using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CaznerMarketplaceBackendApp.Country
{
    public class Regions : FullAuditedEntity<long>
    {
        public string Title { get; set; }
        public string RegionCode { get; set; }

        public Regions()
        {

        }

        public Regions(string title,  string regionCode)
            : this()
        {
            Title = title;
            RegionCode = regionCode;
           
        }
    }

}
