using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductVariantWarehouseDto : EntityDto<long>
    {
        public long WarehouseId { get; set; }
        public string WarehouseTitle { get; set; }
        public long ProductVariantId { get; set; }
        public string LocationA { get; set; }
        public string LocationB { get; set; }
        public string LocationC { get; set; }
        public string StockAlertQuantity { get; set; }
        public string QuantityThisLocation { get; set; }
        public bool IsActive { get; set; }
    }
}
