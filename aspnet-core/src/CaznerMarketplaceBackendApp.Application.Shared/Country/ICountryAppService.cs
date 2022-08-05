using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Country.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Country
{
    public interface ICountryAppService : IAsyncCrudAppService<CountryDto, long, CountryResultRequestDto, CreateOrUpdateCountry, CountryDto>
    {

    }
}
