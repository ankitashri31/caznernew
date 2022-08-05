using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductandPackageDimensionDto : EntityDto<long>
    {           
        public long ProductId { get; set; }
        //package dimension
        public long ProductHeight { get; set; }
        public long ProductWidth { get; set; }
        public long ProductLength { get; set; }
        public long UnitWeight { get; set; }
        public long CartonWeight { get; set; }
        public long CartonQuantity { get; set; }
        //package dimension
        public long PackageHeight { get; set; }
        public long PackageWidth { get; set; }
        public long PackageLength { get; set; }
        public long UnitPerCarton { get; set; }
        public string ProductPackaging { get; set; }
        public string Note { get; set; }
        //Inventory table
        public int StockkeepingUnit { get; set; }
        public string Barcode { get; set; }
        public int TotalNumberAvailable { get; set; }
        public int AlertRestockNumber { get; set; }
        public bool IsTrackQuantity { get; set; }
        public bool IsStopSellingStockZero { get; set; }
        public bool IsActive { get; set; }
    }
}
