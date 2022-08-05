using Abp.Application.Services;
using CaznerMarketplaceBackendApp.ProductBrand.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductBrand
{
    public interface IProductBrandAppService : IAsyncCrudAppService<ProductBrandDto, long, ProductBrandResultRequestDto, CreateOrUpdateProductBrand, ProductBrandDto>
    {
    }
}
