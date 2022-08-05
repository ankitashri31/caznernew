using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductStockLocationDto : EntityDto<long>
    {
       
        public long WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public int QuantityAtLocation { get; set; }
        public int StockAlertQty { get; set; }
        public string LocationA { get; set; }
        public string LocationB { get; set; }
        public string LocationC { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeletedFromUi { get; set; }
    }
}
