using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.Product
{
    public interface IProductAppService : IApplicationService
    {
        Task<ProductMasterDto> CreateProduct(CreateProductDto createProductdto);
    }
}
