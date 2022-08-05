using Abp.Application.Services;
using CaznerMarketplaceBackendApp.ProductCollection.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductCollection
{
    public interface IProductCollectionAppService : IAsyncCrudAppService<ProductCollectionDto, long, ProductCollectionResultRequestDto, CreateOrUpdateProductCollection, ProductCollectionDto>
    {
    }
}
