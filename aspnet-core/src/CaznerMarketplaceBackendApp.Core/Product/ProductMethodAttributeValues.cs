using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductMethodAttributeValues : FullAuditedEntity<long>
    {
        public virtual BrandingMethodMaster BrandingMethod { get; set; }
        [ForeignKey("BrandingMethod")]
        public long BrandingMethodId { get; set; }

        public virtual BrandingAttributeAssignment BrandingAttribute { get; set; }
        [ForeignKey("BrandingAttribute")]
        public long BrandingAttributeAssignmentId { get; set; }

        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductMasterId { get; set; }

        public string Value { get; set; }

        public bool IsActive { get; set; }
    }
}
