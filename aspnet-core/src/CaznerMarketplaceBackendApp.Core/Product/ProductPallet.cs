using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
   public class ProductPallet : FullAuditedEntity<long>,IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }
        public long PalletWeigth { get; set; }
        public long CartonsPerPallet { get; set; }
        public long UnitsPerPallet { get; set; }
        public string Note { get; set; }
        
    }
}
