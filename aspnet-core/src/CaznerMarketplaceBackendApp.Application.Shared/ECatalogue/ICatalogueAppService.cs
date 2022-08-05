using Abp.Application.Services;
using CaznerMarketplaceBackendApp.ECatalogue.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ECatalogue
{
    public interface ICatalogueAppService : IAsyncCrudAppService<ECatalogueDto, long, ECatalogueRequestDto, CreateOrUpdateECatalogue, ECatalogueDto>
    {

    }
}
