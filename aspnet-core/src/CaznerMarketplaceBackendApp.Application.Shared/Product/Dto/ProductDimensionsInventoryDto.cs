using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerAngularCoreApiDemo.Product.Dto
{
    public class ProductDimensionsInventoryDto : EntityDto<long>
    {           
        public long ProductId { get; set; }

        //Product dimension
        public string ProductHeight { get; set; }
        public string ProductWidth { get; set; }
        public string ProductLength { get; set; }
        public string UnitWeight { get; set; }
        public string ProductPackaging { get; set; }
        public string UnitPerProduct { get; set; }
        public string CartonWeight { get; set; }
        public string CartonQuantity { get; set; }
        public long? ProductUnitMeasureId { get; set; }
        public long? ProductWeightMeasureId { get; set; }

        public string ProductUnitMeasureTitle { get; set; }
        public string ProductWeightMeasureTitle { get; set; }
        public string ProductDiameter { get; set; }
        public string ProductDimensionNotes { get; set; }
        //Carton dimension
        public string CartonHeight { get; set; }
        public string CartonWidth { get; set; }
        public string CartonLength { get; set; }
        public string UnitPerCarton { get; set; }
        public string CartonPackaging { get; set; }
        public string CartonNote { get; set; }
        public long? CartonUnitOfMeasureId { get; set; }
        public long? CartonWeightMeasureId { get; set; }
        public string CartonUnitOfMeasureTitle { get; set; }
        public string CartonWeightMeasureTitle { get; set; }
        public string CartonCubicWeightKG { get; set; }
        //Pallet dimension        
        public string PalletWeight { get; set; }
        public string CartonPerPallet { get; set; }
        public string UnitPerPallet { get; set; }
        public string PalletNote { get; set; }
        public string PalletDimension { get; set; }
        //Inventory table
        public string StockkeepingUnit { get; set; }
        public string Barcode { get; set; }
        public string TotalNumberAvailable { get; set; }
        public string AlertRestockNumber { get; set; }
        public bool IsTrackQuantity { get; set; }
        public bool IsStopSellingStockZero { get; set; }
        public bool IsActive { get; set; }
        //   // inventory.Barcode,inventory.AlertRestockNumber, inventory.TotalNumberAvailable, inventory.ProductHeight, inventory.ProductLength, inventory.ProductDiameter,  inventory.ProductDimensionNotes, 
       

    }
}
