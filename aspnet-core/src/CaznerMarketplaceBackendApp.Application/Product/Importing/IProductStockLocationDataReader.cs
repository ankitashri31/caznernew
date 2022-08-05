using System.Collections.Generic;
using CaznerMarketplaceBackendApp.Product.Importing;
using Abp.Dependency;
using CaznerMarketplaceBackendApp.Product.Dto;

namespace CaznerMarketplaceBackendApp.Product.Importing
{
    public interface IProductStockLocationDataReader: ITransientDependency
    {
        List<ImportBulkProductStockLocationDto> GetStockLocationDataFromExcel(byte[] fileBytes);
        List<ImportBulkProductStockLocationDto> GetProductsFromAzureExcel(byte[] fileBytes);
    }
}
