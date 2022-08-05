using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethod
{
    public class BrandingMethodAdditionalPrice : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public int QtyPlus50 { get; set; }
        public int QtyPlus100 { get; set; }
        public int QtyPlus250 { get; set; }
        public int QtyPlus500 { get; set; }
        public int QtyPlus1000 { get; set; }
        public int QtyPlus10000 { get; set; }
        public string Price { get; set; }
        public virtual BrandingMethodMaster BrandingMethod { get; set; }
        [ForeignKey("BrandingMethod")]
        public long BrandingMethodId { get; set; }
        public bool IsActive { get; set; }
    }
}
