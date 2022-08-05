using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethods
{
    public class BrandingAdditionalQtyPrices : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual BrandingAdditionalQuantities BrandingAdditionalQuantities { get; set; }
        [ForeignKey("BrandingAdditionalQuantities")]
        public long AdditionalQtyId { get; set; }
        public double Price  { get; set; }
        public bool IsActive { get; set; }
    }
}
