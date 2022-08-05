using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ArtWork
{
    public class ArtWorkMaster : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string  ArtworkUniqueId { get; set; }
        public string ArtworkSKU { get; set; }
       
        public string ArtworkFeeTitle { get; set; }
        public string ArtworkDescription { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal HandlingCharge { get; set; }
        public string ApprovalDescription { get; set; }
        public string ArtworkNote { get; set; }
        public bool IsEnableForMockups { get; set; }

        public string MockupSKU { get; set; }

        public string MockupUniqueId { get; set; }

        public string MockupTitle { get; set; }
        public string MockupDescription {get; set;}

        public decimal MockupPrice { get; set; }

        public double MaxNumberOfMockUpCanOrder { get; set; }

        public bool IsArtworkEnabled { get; set; }
    }
}
