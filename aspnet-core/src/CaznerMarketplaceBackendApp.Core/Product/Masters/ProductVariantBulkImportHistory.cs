using System;
using System.Collections.Generic;
using System.Text;
using CaznerMarketplaceBackendApp.Product.Masters;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace CaznerMarketplaceBackendApp.Product.Masters
{
    public class ProductVariantBulkImportHistory : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public long ProductId { get; set; }

        public virtual ProductVariantsData ProductVariantsData { get; set; }
        [ForeignKey("ProductVariantsData")]
        public long? VariantDataId { get; set; }

        public virtual ProductVariantOptionValues ProductVariantOptionValues { get; set; }
        [ForeignKey("ProductVariantOptionValues")]
        public long? VariantOptionId { get; set; }

        public virtual ProductBulkUploadVariations ProductBulkUploadVariations { get; set; }
        [ForeignKey("ProductBulkUploadVariations")]
        public long? BulkUploadVariationsId { get; set; }

        public virtual ProductVariantdataImages ProductVariantdataImages { get; set; }
        [ForeignKey("ProductVariantdataImages")]
        public long? VariantdataImageId { get; set; }

        public virtual ProductVariantWarehouse ProductVariantWarehouse { get; set; }
        [ForeignKey("ProductVariantWarehouse")]
        public long? VariantWarehouseId { get; set; }

        //public virtual List<ProductVariantsData>  VariantsDataList { get; set; }

        //public virtual List<ProductVariantOptionValues> VariantOptionValuesList { get; set; }

        //public virtual List<ProductBulkUploadVariations> VariantUploadVariationList { get; set; }

        //public virtual List<ProductVariantdataImages> VariantdataImagesList { get; set; }

        //public virtual List<ProductVariantWarehouse> VariantWarehouseList { get; set; }

    }
}
