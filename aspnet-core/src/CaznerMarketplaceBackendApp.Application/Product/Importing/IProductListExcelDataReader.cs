using System.Collections.Generic;
using CaznerMarketplaceBackendApp.Product.Importing;
using Abp.Dependency;
using CaznerMarketplaceBackendApp.Product.Dto;

namespace CaznerMarketplaceBackendApp.Product.Importing
{
    public interface IProductListExcelDataReader: ITransientDependency
    {
        List<ImportBulkProductDto> GetProductsFromExcel(byte[] fileBytes);
        List<ImportBulkProductDto> GetProductsFromAzureExcel(byte[] fileBytes);
    }
}
