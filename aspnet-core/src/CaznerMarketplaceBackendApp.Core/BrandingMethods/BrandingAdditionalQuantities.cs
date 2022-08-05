using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethods
{
    public class BrandingAdditionalQuantities : FullAuditedEntity<long>, IMustHaveTenant
    {
        public virtual BrandingMethodMaster BrandingMethod { get; set; }
        [ForeignKey("BrandingMethod")]
        public long BrandingMethodId { get; set; }
        public int TenantId { get; set; }
        public double Quantity { get; set; }
        public bool IsActive { get; set; }
    }
}
