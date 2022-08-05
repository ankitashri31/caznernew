using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductMaster : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string ProductSKU { get; set; }
        public string ProductUniqueId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescripition { get; set; }
        public string ShortDescripition { get; set; }
        public decimal Profit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPerItem { get; set; }
        public decimal MarginIncreaseOnSalePrice { get; set; }
        public decimal SalePrice { get; set; }
        public bool OnSale { get; set; }
        public int UnitOfMeasure { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public decimal DepositRequired { get; set; }
        public bool ChargeTaxOnThis { get; set; }
        public bool ProductHasPriceVariant { get; set; }

        //public int PriceChargeTax { get; set; }
        public bool IsActive { get; set; }
        public bool IsProductHasMultipleOptions { get; set; }
        public bool IsPhysicalProduct { get; set; }
        public string ProductNotes { get; set; }
        public string Features { get; set; }
        public string ColorsAvailable { get; set; }
        public bool IsPublished { get; set; }
        public double DiscountPercentage { get; set; }
        public double DiscountPercentageDraft { get; set; }
        public bool IsProductHasMixAndMatch { get; set; }

        public virtual TurnAroundTime TurnAroundTime { get; set; }
        [ForeignKey("TurnAroundTime")]
        public long? TurnAroundTimeId { get; set; }
        public string FrightNote { get; set; }
        public string VolumeValue { get; set; }
        public string VolumeUOM { get; set; }
        public string IfSetOtherProductTitleAndDimensoionsInThisSet { get; set; }
        public string CounrtyOfOrigin { get; set; }
        public string NumberOfPieces { get; set; }
        public bool IsIndentOrder { get; set; }
        public string ColourFamily { get; set; }
        public string PMSColourCode { get; set; }
        public string VideoURL { get; set; }
        public string Image360Degrees { get; set; }
        public string NextShipmentDate { get; set; }
        public string NextShipmentQuantity { get; set; }
        public string ExtraSetUpFee { get; set; }
        public string BrandingMethodNote { get; set; }
        public string BrandingUOM { get; set; }
        public bool IsHasSubProducts { get; set; }
        public int NumberOfSubProducts { get; set; }
        public bool IsProductHasCompartmentBuilder { get; set; }
        public string CompartmentBuilderTitle { get; set; }

        public bool IsProductIsCompartmentType { get; set; }
        
    }
}
