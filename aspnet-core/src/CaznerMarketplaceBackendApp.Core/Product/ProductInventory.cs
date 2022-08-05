using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductInventory : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }
        public int StockkeepingUnit { get; set; }
        public string Barcode { get; set; }
        public int TotalNumberAvailable { get; set; }
        public int AlertRestockNumber { get; set; }
        public bool IsTrackQuantity { get; set; }
        public bool IsStopSellingStockZero { get; set; }
        public bool IsActive { get; set; }
    }
}
