using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Masters
{
    public class ProductOptionsMaster : FullAuditedEntity<long>
    {
        public string OptionName { get; set; }
        public bool IsActive { get; set; }
    }
}
