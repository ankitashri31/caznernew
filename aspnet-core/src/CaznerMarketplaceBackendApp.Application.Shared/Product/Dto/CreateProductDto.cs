using Abp.Application.Services.Dto;
using CaznerAngularCoreApiDemo.Product.Dto;
using CaznerMarketplaceBackendApp.BrandingMethod.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class CreateProductDto : EntityDto<long>
    {
        [Required]
        public string ProductSKU { get; set; }
        //[Required]
        //public string ProductUniqueId { get; set; }
        [Required]
        public string ProductTitle { get; set; }
        [Required]
        public string ProductDescripition { get; set; }
        public string ShortDescripition { get; set; }
        public string ProductNotes { get; set; }
        [Required]
        public ProductPriceDto ProductPrice { get; set; }
        public ProductDetailDto ProductDetail { get; set; }
        public string Features { get; set; }
        public long? BrandingUnitOfMeasureId { get; set; }

        public List<ProductBrandingPositionDto> ProductBrandingPosition { get; set; }

        public List<ProductVolumeDiscountVariantDto> ProductVolumeDiscountVariant { get; set; }
        public List<ProductStockLocationDto> ProductStockLocation { get; set; }
        public ProductDimensionsInventoryDto ProductDimensionsInventory { get; set; }
        public List<ProductBulkUploadVariationsDto> ProductBulkUploadVariations { get; set; }
        public List<ProductVariantDataDto> ProductVariantData { get; set; }
        public List<BrandingMethodDto> BrandingMethodData { get; set; }

        public List<ProductBrandingPriceVariantsDto> ProductBrandingPriceVariants { get; set; }

        public bool IsActive { get; set; }
        public bool IsProductHasMultipleOptions { get; set; }
        public bool IsPhysicalProduct { get; set; }
        public bool IsPublished { get; set; }
        public bool IsIndentOrder { get; set; }
        public ProductMediaType ProductDefaultImage { get; set; }
        public List<ProductMediaType> ProductImagesNames { get; set; }
        public List<ProductMediaType> ProductMediaImages { get; set; }
        public long? TurnAroundTimeId { get; set; }
        public bool IsHasSubProducts { get; set; }
        public int NumberOfSubProducts { get; set; }
        public bool IsProductHasCompartmentBuilder { get; set; }
        public bool IsProductCompartmentType { get; set; }
        public List<CompartmentVariantDataDto> CompartmentVariantData { get; set; }
        public ProductMediaType CompartmentBaseImage { get; set; }
        public string CompartmentBuilderTitle { get; set; }
        public List<long> SubCategoryId { get; set; }

    }

    public class ProductMediaType
    {
        public string FileName { get; set; }
        public int MediaType { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class ProductMediaModel
    {
        public long MediaType { get; set; }
        public string MediaTypeName { get; set; }
        public ProductImageType[] Images { get; set; }
    }

    public class ProductImageType
    {
        public string ProductSKU { get; set; }
        public long Id { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public string ImagePath { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public long MediaType { get; set; }
        public bool IsDefaultImage { get; set; }
        public bool IsVariantImage { get; set; }
        public string BaseImageByteData { get; set; }
        public double QuantityStockUnit { get; set; }
        public string TermsAndCondition { get; set; }
        public bool IsHideContactDetails { get; set; }
        public string FaviconImageURL { get; set; }
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string PinterestLink { get; set; }
        public string TwitterLink { get; set; }
    }


    public class ProductImageDto
    {
        public string ImageName { get; set; }
        public long Id { get; set; }
    }
    public class ProductMediaImageDto
    {
        public int MediaType { get; set; }
        public string FileName { get; set; }
        public long Id { get; set; }
        public string Msg { get; set; }
    }

    public class ProductMediaImageRequest
    {
        public int MediaType { get; set; }
        public byte[] FileData { get; set; }
        public string Extension { get; set; }
    }

    public class ProductDetailCombo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long AssignmentId { get; set; }
    }
    public class ProductCategory
    {
        public long ProductId { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long SubCategoryId { get; set; }
        public long AssignmentCategoryId { get; set; }
        public long AssignnmentSubCategoryId { get; set; }

        public long AssignnmentSubSubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public long SubSubCategoryId { get; set; }
        public string SubSubCategoryName { get; set; }
        public int SubCount { get; set; }
        public int SubSubCount { get; set; }
    }


    public class ProductImageRequest
    {
        public byte[] ImageData { get; set; }
        public string Extension { get; set; }
    }

    public class GetProductDto : EntityDto<long>
    {
        [Required]
        public string ProductSKU { get; set; }
        //[Required]
        //public string ProductUniqueId { get; set; }
        [Required]
        public string ProductTitle { get; set; }
        [Required]
        public string ProductDescripition { get; set; }
        public string ShortDescripition { get; set; }
        public string ProductNotes { get; set; }
        public long? TurnAroundTimeId { get; set; }
        public string TurnAroundTime { get; set; }
        public string Features { get; set; }

        [Required]
        public ProductPriceDto ProductPrice { get; set; }
        public ProductDetailComboDto ProductDetail { get; set; }
        public List<ProductBrandingPositionDto> ProductBrandingPosition { get; set; }

        public List<ProductVolumeDiscountVariantDto> ProductVolumeDiscountVariant { get; set; }
        public List<ProductStockLocationDto> ProductStockLocation { get; set; }
        public ProductDimensionsInventoryDto ProductDimensionsInventory { get; set; }
        public List<ProductBrandingPriceVariantsDto> ProductBrandingPriceVariants { get; set; }

        public List<ProductVariantValueDto> ProductVariantValues { get; set; }

        public List<BrandingMethodDto> BrandingMethodData { get; set; }
        public List<ProductVariantDataDto> ProductVariantData { get; set; }

        public bool IsActive { get; set; }
        public bool IsProductHasMultipleOptions { get; set; }
        public bool IsPhysicalProduct { get; set; }
        public bool IsHasPriceVariants { get; set; }
        public bool IsPublished { get; set; }
        public bool IsIndentOrder { get; set; }
        public ProductImageType ProductDefaultImage { get; set; }
        public List<ProductImageType> ProductImagesNames { get; set; }
        public List<ProductMediaModel> ProductMediaImages { get; set; }
        public List<ProductImageType> LineArtMediaImages { get; set; }
        public List<ProductImageType> OtherMediaImages { get; set; }
        public List<ProductImageType> LifeStyleMediaImages { get; set; }

        public long? BrandingUnitOfMeasureId { get; set; }

        public string BrandingUnitOfMeasure { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
        public string Style { get; set; }
        public bool IsHasSubProducts { get; set; }
        public int NumberOfSubProducts { get; set; }

        public ProductImageType CompartmentBaseImage { get; set; }

        public List<CompartmentVariantDataDto> CompartmentVariantData { get; set; }

        public bool IsProductHasCompartmentBuilder { get; set; }
        public bool IsProductCompartmentType { get; set; }
        public string CompartmentTile { get; set; }
        public string CompartmentDescription { get; set; }
        public string CompartmentBuilderTitle { get; set; }
        public List<AllCategoryIdDto> ProductCategories { get; set; }
    }

    public class GetCompartmentVarientDataListDto : EntityDto<long>
    {

        public long ProductId { get; set; }

        public string CompartmentTitle { get; set; }
        public string CompartmentSubTitle { get; set; }
        public Varient VarientList { get; set; }

    }
    public class Varient
    {
        public decimal Price { get; set; }
        public ProductImageType ImageObj { get; set; }
        public string SKU { get; set; }
        public long? ProductVarientId { get; set; }
    }

    public class GetProductVariantMediaDto
    {

        public List<ProductBrandingPositionDto> ProductBrandingPosition { get; set; }

        public List<ProductVariantValueDto> ProductVariantValues { get; set; }

        public List<ProductMediaModel> ProductMediaImages { get; set; }
        public List<ProductImageType> LineArtMediaImages { get; set; }
        public List<ProductImageType> OtherMediaImages { get; set; }
        public List<ProductImageType> LifeStyleMediaImages { get; set; }

    }

    public class VariantModel
    {
        public long Id { get; set; }
        public string Color { get; set; }
        public string ProductImage { get; set; }
        public List<VariantValues> Variant { get; set; }

    }
    public class VariantDataColor
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public double QuantityStockUnit { get; set; }
        public string Variant { get; set; }
        public string VariantMasterIds { get; set; }
        public string ImageURL { get; set; }

    }
    public class VariantValues
    {
        public string Sizes { get; set; }
        public decimal Price { get; set; }
        public double Quantity { get; set; }
        public long Id { get; set; }
    }


    public class GetProductFrontEndDto : EntityDto<long>
    {
        [Required]
        public string ProductSKU { get; set; }
        //[Required]
        //public string ProductUniqueId { get; set; }
        [Required]
        public string ProductTitle { get; set; }
        [Required]
        public string ProductDescripition { get; set; }
        public string ShortDescripition { get; set; }
        public string ProductNotes { get; set; }
        public string TurnAroundTime { get; set; }
        public string Features { get; set; }

        [Required]
        public ProductPriceDto ProductPrice { get; set; }
        public ProductDetailFrontendComboDto ProductDetail { get; set; }
        public List<ProductBrandingPositionDto> ProductBrandingPosition { get; set; }

        public List<ProductVolumeDiscountVariantDto> ProductVolumeDiscountVariant { get; set; }
        public List<ProductStockLocationDto> ProductStockLocation { get; set; }
        public ProductDiamensionsCombo ProductDimensionsInventory { get; set; }
        public List<ProductBrandingPriceVariantsDto> ProductBrandingPrice { get; set; }

        public List<ProductVariantValueDto> ProductVariantValues { get; set; }
        public List<BrandingMethodDto> BrandingMethodData { get; set; }

        public List<VariantModel> VariantColorSizeModel { get; set; }
        public bool IsIndentOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsProductHasMultipleOptions { get; set; }
        public bool IsPhysicalProduct { get; set; }
        public bool IsPublished { get; set; }
        public ProductImageType ProductDefaultImage { get; set; }
        public List<ProductImageType> ProductImagesNames { get; set; }

        public List<ProductImageType> ProductMainAndVariantImages { get; set; }
        public List<ProductMediaModel> ProductMediaImages { get; set; }
        public List<BrandingMethodPriceModel> ProductBrandingPriceVariants { get; set; }

        public List<BrandingMethodPriceModel> AdditionalBrandingPriceVariants { get; set; }
        public long? BrandingUnitOfMeasureId { get; set; }
        public string BrandingUnitOfMeasure { get; set; }
        public bool IsHasSubProducts { get; set; }
        public int NumberOfSubProducts { get; set; }
        public decimal ArtWorkUnitPrice { get; set; }
        public decimal ArtWorkHandlingCharges { get; set; }
        public ProductArtWorkDto ProductArtWorkDto { get; set; }
        public SubProductDto SubProductDto { get; set; }

        public bool IsProductIsCompartmentType { get; set; }
        public bool IsProductHasCompartmentBuilder { get; set; }
        public bool IsShowPriceWithoutAccount { get; set; }
        public string NumberOfPieces { get; set; }

    }
    public class ProductArtWorkDto
    {
        public decimal ArtWorkActualUnitPrice { get; set; }
        public decimal ArtWorkUnitPrice { get; set; }
        public decimal ArtWorkHandlingCharges { get; set; }
    }
    public class SubProductDto
    {
        public bool IsHasSubProducts { get; set; }
        public int NumberOfSubProducts { get; set; }
    }

    public class ProductDiamensionsCombo
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public PalletDimension PalletDimension { get; set; }
        public CartonDimension CartonDimension { get; set; }
        public UnitDimensions UnitDimension { get; set; }
    }

    public class PalletDimension
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        //Pallet dimension        
        public string PalletWeight { get; set; }
        public string CartonPerPallet { get; set; }
        public string UnitPerPallet { get; set; }
        public string PalletNote { get; set; }

        public bool IsActive { get; set; }
    }

    public class CartonDimension
    {
        //Carton dimension
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string CartonHeight { get; set; }
        public string CartonWidth { get; set; }
        public string CartonLength { get; set; }
        public string UnitPerCarton { get; set; }
        public string CartonWeight { get; set; }
        public string CartonPackaging { get; set; }
        public string CartonNote { get; set; }
        public bool IsActive { get; set; }
        public long? CartonUnitOfMeasureId { get; set; }
        public long? CartonWeightMeasureId { get; set; }
        public string CartonUnitOfMeasureTitle { get; set; }
        public string CartonWeightMeasureTitle { get; set; }
        public string CartonCubicWeightKG { get; set; }
    }

    public class UnitDimensions
    {
        //Product dimension
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ProductHeight { get; set; }
        public string ProductWidth { get; set; }
        public string ProductLength { get; set; }
        public string UnitWeight { get; set; }
        public string ProductPackaging { get; set; }
        public string UnitPerProduct { get; set; }
        public string CartonWeight { get; set; }
        public string CartonQuantity { get; set; }
        public bool IsActive { get; set; }
        public long? ProductUnitMeasureId { get; set; }
        public long? ProductWeightMeasureId { get; set; }

        public string ProductUnitMeasureTitle { get; set; }
        public string ProductWeightMeasureTitle { get; set; }
        public string ProductDiameter { get; set; }
        public string ProductDimensionNotes { get; set; }

    }
}
