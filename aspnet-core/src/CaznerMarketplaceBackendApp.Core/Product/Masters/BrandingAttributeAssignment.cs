using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Masters
{
    public class BrandingAttributeAssignment : FullAuditedEntity<long>
    {
        public virtual BrandingMethodMaster BrandingMethod { get; set; }
        [ForeignKey("BrandingMethod")]
        public long BrandingMethodId { get; set; }

        public virtual BrandingMethodAttributes BrandingMethodAttr { get; set; }
        [ForeignKey("BrandingMethodAttr")]
        public long BrandingMethodAttributeId { get; set; }
        public string AttributeValue { get; set; }

        public bool IsActive { get; set; }

    }
}
