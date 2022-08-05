using System;
using System.Collections.Generic;
using System.Text;
using Abp.Dependency;
using CaznerMarketplaceBackendApp.Product.Dto;

namespace CaznerMarketplaceBackendApp.Product.Importing
{
    public interface IVariantStockLocationDataReader : ITransientDependency
    {
        List<ImportVariantBulkStockLocDto> GetStockLocationDataFromExcel(byte[] fileBytes);
        List<ImportVariantBulkStockLocDto> GetProductsFromAzureExcel(byte[] fileBytes);
    }
}
