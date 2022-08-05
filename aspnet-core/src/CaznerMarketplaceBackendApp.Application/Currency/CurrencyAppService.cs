using Abp.Application.Services;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.Currency.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Currency
{
    public class CurrencyAppService : AsyncCrudAppService<CurrencyMaster, CurrencyDto, long, CurrencyResultRequestDto, CreateOrUpdateCurrency, CurrencyDto>, ICurrencyAppService
    {
        public CurrencyAppService(IRepository<CurrencyMaster, long> repository) : base(repository)
        {
        }
    }
}
