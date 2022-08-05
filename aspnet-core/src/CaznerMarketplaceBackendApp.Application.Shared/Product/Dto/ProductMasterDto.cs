using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductMasterDto : EntityDto<long>
    {
        public string ErrorMessage { get; set; }
        public bool IsValidate { get; set; }
        public string ProductSKU { get; set; }
        public string ProductUniqueId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescripition { get; set; }
        public string ShortDescripition { get; set; }
        public decimal Profit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPerItem { get; set; }
        public decimal MarginIncreaseOnSalePrice { get; set; }
        public decimal SalePrice { get; set; }
        public bool OnSale { get; set; }
        public int UnitOfMeasure { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public decimal DepositRequired { get; set; }
        public bool IsChargesTax { get; set; }
        public bool IsProductHasMultipleOptions { get; set; }
        public bool IsPhysicalProduct { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductMasterResultRequestDto : PagedResultRequestDto
    {
        public string SearchText { get; set; }
        public int? TagId { get; set; }
        public int? TypeId { get; set; }
        public int? CollectionId { get; set; }
        public List<long> SubCategoryIds { get; set; }
        public List<long> SubSubCategoryIds { get; set; }
        public int? CategoryId { get; set; }
        public int? Sorting { get; set; }
        public int? FilterBy { get; set; }
        public string EncryptedTenantId { get; set; }

    }
    public class ProductCustomlistDto : PagedResultRequestDto
    {
        public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<ProductListViewDto> items { get; set; }
    }

    public class ProductViewDto : PagedResultRequestDto
    {
        public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<ProductView> items { get; set; }
    }

    public class ProductCustomlistFrontEndDto : PagedResultRequestDto
    {
        public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<CollectionproductView> items { get; set; }
    }

    //public class ProductDataRequestDto 
    //{     
    //    public int ProductId { get; set; }
    //    public string EncryptedTenantId{ get; set; }
    //}

    public class ProductDataRequestDto
    {
        public int ProductId { get; set; }
        public string EncryptedTenantId { get; set; }

    }
    public class ProductBrandingPriceVsDto
    {
        public long Id { get; set; }

        public long BrandingId { get; set; }
        public string MethodName { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsColorSelected { get; set; }
        public string SelectedColor { get; set; }
        public string MaxSelectedColor { get; set; }
        public string Quantity { get; set; }
        public string BrandingUnitPrice { get; set; }
        public string Price { get; set; }
        public bool IsActive { get; set; }
        public long PriceVarientId { get; set; }
        public long? BrandingMethodId { get; set; }

    }

    public class UpdatePriceModel
    {
        public long Id { get; set; }
        public decimal Percent { get; set; }

        public string Type { get; set; }
    }
}
