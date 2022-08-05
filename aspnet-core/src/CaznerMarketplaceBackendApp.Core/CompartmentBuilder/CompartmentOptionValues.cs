using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class CompartmentOptionValues : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual CompartmentVariantData ProductCompartmentData { get; set; }
        [ForeignKey("ProductCompartmentData")]
        public long CompartmentVariantId { get; set; }
        public virtual ProductOptionsMaster ProductOptions { get; set; }
        [ForeignKey("ProductOptions")]
        public long OptionId { get; set; }
        public string CompartmentOptionValue { get; set; }
        public bool IsActive { get; set; }

    }
}
