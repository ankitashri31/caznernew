using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductDimensionsInventory: FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }

        //Product dimension
        public string ProductHeight { get; set; }
        public string ProductWidth { get; set; }
        public string ProductLength { get; set; }
        public string ProductDiameter { get; set; }
        public string ProductDimensionNotes { get; set; }
        public string UnitWeight { get; set; }

        public virtual ProductSizeMaster ProductSizeMaster { get; set; }
        [ForeignKey("ProductSizeMaster")]
        public long? ProductUnitMeasureId { get; set; }

        public virtual ProductSizeMaster ProductSize { get; set; }
        [ForeignKey("ProductSize")]
        public long? ProductWeightMeasureId { get; set; }
        public string ProductPackaging { get; set; }
        public double UnitPerProduct { get; set; }
        public string CartonWeight { get; set; }
        public string CartonQuantity { get; set; }

        //Carton dimension
        public string CartonHeight { get; set; }
        public string CartonWidth { get; set; }
        public string CartonLength { get; set; }
        public double UnitPerCarton { get; set; }
        public string CartonPackaging { get; set; }
        public string CartonNote { get; set; }

        public virtual ProductSizeMaster ProductCartonSize { get; set; }
        [ForeignKey("ProductCartonSize")]
        public long? CartonUnitOfMeasureId { get; set; }

        public virtual ProductSizeMaster ProductCarton { get; set; }
        [ForeignKey("ProductCarton")]
        public long? CartonWeightMeasureId { get; set; }
        public string CartonCubicWeightKG { get; set; }

        //Pallet dimension        
        public string PalletWeight { get; set; }
        public string PalletDimension{ get; set; }
        public double CartonPerPallet { get; set; }
        public double UnitPerPallet { get; set; }
        public string PalletNote { get; set; }

        //Inventory table
        public int StockkeepingUnit { get; set; }
        public string Barcode { get; set; }
        public long TotalNumberAvailable { get; set; }
        public int AlertRestockNumber { get; set; }
        public bool IsTrackQuantity { get; set; }
        public bool IsStopSellingStockZero { get; set; }
        public bool IsActive { get; set; }

    }
}