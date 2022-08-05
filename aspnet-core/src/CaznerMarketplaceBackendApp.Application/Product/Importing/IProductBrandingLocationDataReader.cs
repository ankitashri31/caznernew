using System;
using System.Collections.Generic;
using System.Text;
using CaznerMarketplaceBackendApp.Product.Dto;
using Abp.Dependency;

namespace CaznerMarketplaceBackendApp.Product.Importing
{
    public interface IProductBrandingLocationDataReader : ITransientDependency
    {
        List<ImportBulkBrandingLocationDto> GetStockLocationDataFromExcel(byte[] fileBytes);
        List<ImportBulkBrandingLocationDto> GetProductsFromAzureExcel(byte[] fileBytes);
    }
}
