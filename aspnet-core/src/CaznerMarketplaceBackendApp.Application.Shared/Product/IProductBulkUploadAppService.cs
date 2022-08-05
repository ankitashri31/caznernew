using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.Product
{
    public interface IProductBulkUploadAppService : IApplicationService
    {
        Task ProductImport(Guid ObjectId);
        Task ProductVariantImport(Guid ObjectId);
        Task ProductStockLocationImport(Guid ObjectId);
        Task <ImportProductModel> GetMetadataFromExcelOrNull(Guid BinaryObjectId);
        Task CreateProductData(List<ImportBulkProductDto> productData);

        Task CreateProductStockLocationDataImport(List<ImportBulkProductStockLocationDto> stockLocation);
        Task<List<ImportBulkProductStockLocationDto>> GetStockLocationDataFromExcelOrNull(Guid BinaryObjectId);

        Task ProductBrandingPositionImport(Guid ObjectId);
        Task CreateProductBrandingPositionDataImport(List<ImportBulkBrandingLocationDto> BrandingPositionData);
        Task<List<ImportBulkBrandingLocationDto>> GetBrandingPositionDataFromExcelOrNull(Guid BinaryObjectId);

        Task ProductVariantStockLocationImport(Guid ObjectId);
        Task<List<ImportVariantBulkStockLocDto>> GetVariantStockLocFromExcelOrNull(Guid BinaryObjectId);

        Task CreateVariantStockLocationDataImport(List<ImportVariantBulkStockLocDto> stockLocation);

    }
}
