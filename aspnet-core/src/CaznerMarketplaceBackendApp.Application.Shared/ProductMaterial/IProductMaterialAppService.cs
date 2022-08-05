using Abp.Application.Services;
using CaznerMarketplaceBackendApp.ProductMaterial.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductMaterial
{
    public interface IProductMaterialAppService: IAsyncCrudAppService<ProductMaterialDto, long, ProductMaterialResultRequestDto, CreateOrUpdateProductMaterial, ProductMaterialDto>
    {
    }
}
