using System.Collections.Generic;
using CaznerMarketplaceBackendApp.Product.Importing;
using Abp.Dependency;
using CaznerMarketplaceBackendApp.Product.Dto;

namespace CaznerMarketplaceBackendApp.Product.Importing
{
    public interface IProductVariantExcelDataReader : ITransientDependency
    {
        List<ImportColorVariantsDto> GetProductsVariantFromExcel(byte[] fileBytes);
    }
}
