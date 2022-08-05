using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
   public class ProductStockLocation : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }
        public long WareHouseId { get; set; }
        public int QuantityAtLocation { get; set; }
        public int StockAlertQty { get; set; }
        public string LocationA { get; set; }
        public string LocationB { get; set; }
        public string LocationC { get; set; }
        public bool IsActive { get; set; }
    }
}
