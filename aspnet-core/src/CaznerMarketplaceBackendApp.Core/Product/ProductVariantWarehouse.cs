using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.WareHouse;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductVariantWarehouse : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual WareHouseMaster WareHouseMaster { get; set; }
        [ForeignKey("WarehouseMaster")]
        public long WarehouseId { get; set; }

        public virtual ProductVariantsData ProductVariantsData { get; set; }
        [ForeignKey("ProductVariantsData")]
        public long ProductVariantId { get; set; }
        public string LocationA { get; set; }
        public string LocationB { get; set; }
        public string LocationC { get; set; }
        public double StockAlertQuantity { get; set; }
        public double QuantityThisLocation { get; set; }
        public bool IsActive { get; set; }
    }
}
