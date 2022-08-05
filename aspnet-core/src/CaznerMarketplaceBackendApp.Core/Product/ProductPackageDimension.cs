using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductPackageDimension : FullAuditedEntity<long>, IMustHaveTenant
    {        
        public int TenantId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }
        public long Height { get; set; }
        public long Width { get; set; }
        public long Length { get; set; }
        public long UnitPerCarton { get; set; }
        public string ProductPackaging { get; set; }
        public string Note { get; set; }





    }
}
