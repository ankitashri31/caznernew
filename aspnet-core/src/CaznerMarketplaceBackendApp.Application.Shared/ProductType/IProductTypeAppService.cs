using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services;
using CaznerMarketplaceBackendApp.ProductType.Dto;

namespace CaznerMarketplaceBackendApp.ProductType
{
    public interface IProductTypeAppService : IAsyncCrudAppService<ProductTypeDto, long, ProductTypeResultRequestDto, CreateOrUpdateProductType, ProductTypeDto>
    {
    }
}
