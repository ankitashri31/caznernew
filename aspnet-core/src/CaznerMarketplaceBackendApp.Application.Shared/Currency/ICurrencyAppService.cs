using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Currency.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Currency
{
    public interface ICurrencyAppService:IAsyncCrudAppService<CurrencyDto, long, CurrencyResultRequestDto, CreateOrUpdateCurrency, CurrencyDto>
    {
    }
}
