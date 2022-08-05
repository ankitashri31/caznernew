using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductInventoryDto : EntityDto<long>
    {
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
