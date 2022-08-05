using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.ProductSize.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Currency.Dto
{
    public class CurrencyDto : EntityDto<long>
    {
        public string CurrencyName { get; set; }
        public bool IsActive { get; set; }
    }

    public class CurrencyResultRequestDto : PagedResultRequestDto
    {
        public string CurrencyName { get; set; }
        public string Sorting { get; set; }

    }

    public class CurrencySizeDto
    {
        public List<CurrencyDto> CurrencyData { get; set; }
        public List<ProductSizeDto> ProductSizeData { get; set; }
    }
        
}
