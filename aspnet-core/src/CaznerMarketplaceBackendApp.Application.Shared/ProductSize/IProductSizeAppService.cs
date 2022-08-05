using Abp.Application.Services;
using CaznerMarketplaceBackendApp.ProductSize.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductSize
{
    public interface IProductSizeAppService: IAsyncCrudAppService<ProductSizeDto, long, ProductSizeResultRequestDto, CreateOrUpdateProductSize, ProductSizeDto>
    {
    }
}
