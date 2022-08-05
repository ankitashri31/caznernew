using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class CompartmentVariantData : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }

        public virtual ProductVariantsData ProductVariant { get; set; }
        [ForeignKey("ProductVariant")]
        public long? ProductVarientId { get; set; }


        public string Compartment { get; set; }
        public string CompartmentMasterIds { get; set; }
        public string SKU { get; set; }

        public string CompartmentTitle { get; set; }
        public string CompartmentSubTitle { get; set; }

        public decimal Price { get; set; }

        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public string Url { get; set; }
        public string ImagePath { get; set; }
        public string ImageFileName { get; set; }

    }
}
