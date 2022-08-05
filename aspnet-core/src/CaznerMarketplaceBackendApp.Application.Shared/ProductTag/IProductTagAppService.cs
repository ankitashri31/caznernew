using Abp.Application.Services;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.ProductTag.Dto;
using CaznerMarketplaceBackendApp.Tag.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductTag
{
    public interface IProductTagAppService : IAsyncCrudAppService<ProductTagDto, long, ProductTagResultRequestDto, CreateOrUpdateProductTag, ProductTagDto>
    {
    }
}
