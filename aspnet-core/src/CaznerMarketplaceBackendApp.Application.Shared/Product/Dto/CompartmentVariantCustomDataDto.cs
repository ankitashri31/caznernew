using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class CompartmentVariantCustomDataDto : EntityDto<long>
    {
        public int TenantId { get; set; }
        public long ProductId { get; set; }
        public long? ProductVarientId { get; set; }


        public string Compartment { get; set; }
        public string CompartmentMasterIds { get; set; }
        public string SKU { get; set; }

        public string CompartmentTitle { get; set; }
        public string CompartmentSubTitle { get; set; }

        public decimal Price { get; set; }

        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public string Url { get; set; }
        public string ImagePath { get; set; }
        public string ImageFileName { get; set; }
        public string Variant { get; set; }
        public string Color { get; set; }
        public string ProductName { get; set; }
        public string ProductTitle { get; set; }

    }
}
