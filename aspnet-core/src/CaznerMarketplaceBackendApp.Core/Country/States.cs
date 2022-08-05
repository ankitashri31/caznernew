using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Country;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Country
{
    public class States : FullAuditedEntity<long>
    {
        public string StateName { get; set; }
        public virtual Countries Country { get; set; }
        [ForeignKey("Country")]
        public long CountryId { get; set; }
        public bool IsActive { get; set; }
    }
}
