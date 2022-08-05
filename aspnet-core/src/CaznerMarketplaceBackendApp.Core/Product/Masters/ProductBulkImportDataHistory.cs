using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Masters
{
    public class ProductBulkImportDataHistory : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public string SKU { get; set; }

        public bool IsImportDone { get; set; }

        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductMasterId { get; set; }

        public virtual ProductDetails ProductDetails { get; set; }
        [ForeignKey("ProductDetails")]
        public long? ProductDetailsId { get; set; }

        public virtual ProductDimensionsInventory ProductDimensionsInventory { get; set; }
        [ForeignKey("ProductDimensionsInventory")]
        public long? ProductInventoryId { get; set; }

        public virtual ProductDimension ProductDimension { get; set; }
        [ForeignKey("ProductDimension")]
        public long? ProductDimensionId { get; set; }

        public virtual ProductImages ProductImages { get; set; }
        [ForeignKey("ProductImages")]
        public long? ProductImagesId { get; set; }

        public virtual ProductMediaImages ProductMediaImages { get; set; }
        [ForeignKey("ProductMediaImages")]
        public long? ProductMediaImagesId { get; set; }

        public virtual ProductVolumeDiscountVariant ProductVolumeDiscountVariant { get; set; }
        [ForeignKey("ProductVolumeDiscountVariant")]
        public long? ProductVolumeDiscountVariantId { get; set; }

        public virtual ProductBrandingPriceVariants ProductBrandingPriceVariants { get; set; }
        [ForeignKey("ProductBrandingPriceVariants")]
        public long? PriceVariantsId { get; set; }

        public bool IsActive { get; set; }

        public virtual List<ProductImages> ProductImagesList { get; set; }

        public virtual List<ProductBrandingPriceVariants> BrandingPriceVariantsList { get; set; }
    }

}

