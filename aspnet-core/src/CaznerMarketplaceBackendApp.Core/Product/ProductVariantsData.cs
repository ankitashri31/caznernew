using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.WareHouse;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductVariantsData : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }        
        public string Variant { get; set; }
        public string VariantMasterIds { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public string NextShipment { get; set; }
        public decimal IncomingQuantity { get; set; }
        public decimal ComparePrice { get; set; }
        public decimal CostPerItem { get; set; }
        public string Margin { get; set; }
        public int ProfitCurrencySymbol { get; set; }
        public bool IsChargeTaxVariant { get; set; }
        public decimal Profit { get; set; }
        public string BarCode { get; set; }
        public double QuantityStockUnit { get; set; }
        public bool IsTrackQuantity { get; set; } 
        public bool IsActive { get; set; }
        public string Shape { get; set; }
        public bool IsVariantHasMixAndMatch { get; set; }
        public bool IsMultiColorVariant { get; set; }
        public decimal SalePrice { get; set; }
        public bool OnSale { get; set; }
        public double DiscountPercentage { get; set; }
        public double DiscountPercentageDraft { get; set; }
        public decimal SaleUnitPrice { get; set; }
    }
}
