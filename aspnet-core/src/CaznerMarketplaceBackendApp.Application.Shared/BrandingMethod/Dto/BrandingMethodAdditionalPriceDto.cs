using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethod.Dto
{
    public class BrandingMethodAdditionalPriceDto:EntityDto<long>
    {
        public int TenantId { get; set; }
        public int QtyPlus50 { get; set; }
        public int QtyPlus100 { get; set; }
        public int QtyPlus250 { get; set; }
        public int QtyPlus500 { get; set; }
        public int QtyPlus1000 { get; set; }
        public int QtyPlus10000 { get; set; }
        public string Price { get; set; }
        public long BrandingMethodId { get; set; }
        public bool IsActive { get; set; }
    }
}
