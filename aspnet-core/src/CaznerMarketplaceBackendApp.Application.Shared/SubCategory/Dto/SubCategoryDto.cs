using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.SubCategory
{
    public class SubCategoryDto : EntityDto<long>
    {

        public List<CategoryModelData> CategoryIds { get; set; }

        public long CategoryId { get; set; }
        public string SubCategoryTitle { get; set; }
        public long CategorySubCategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string ImagePath { get; set; }
        public ProductImageType ImageObj { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }


    public class AllCategoryResponseDto : EntityDto<long>
    {
        public long SubCategoryId { get; set; }
        public string SubCategoryTitle { get; set; }
        public long SubSubCategoryId { get; set; }
        public string SubSubCategoryTitle { get; set; }
        public long CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public int SequenceNumber { get; set; }
        public string ImagePath { get; set; }
        public ProductImageType ImageObj { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public int SkipCount { get; set; }
        public List<AllCategoryResponseDto> items { get; set; }
        public DateTime CreationTime { get; set; }
        public List<ProductListDto> product { get; set; }
    }


    public class SubCategoryResultRequestDto : PagedResultRequestDto
    {
        public long? SubCategoryId { get; set; }
        public int? Sorting { get; set; }
        public int? FilterBy { get; set; }
        public string SearchText { get; set; }
        public string EncryptedTenantId { get; set; }

    }
    public class SubSubCategoryResultRequestDto
    {
        public int? SubCategoryId { get; set; }
        public int? Sorting { get; set; }
        public int? FilterBy { get; set; }
        public string EncryptedTenantId { get; set; }

    }
    public class SubSubCategoryDto
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string SubSubCategory { get; set; }
        public string ImageUrl { get; set; }
    }
    public class CategoryModelData
    {
        public long Id { get; set; }
        public long CategorySubCategoryId { get; set; }
        public string CategoryTitle { get; set; }
    }
    public class SubCategoryCustomlistDto : PagedResultRequestDto
    {
        //public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<AllCategoryResponseDto> items { get; set; }
    }

    public class CategoryCustomDto : PagedResultRequestDto
    {
        //public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<CategoryCustomDataDto> items { get; set; }
    }

    
    public class ProductDataRequestBySubSubCategoryDto : PagedResultRequestDto
    {
        public string SearchText { get; set; }
        public long SubSubCategoryId { get; set; }
        public string EncryptedTenantId { get; set; }
        public int? Sorting { get; set; }
        public int? FilterBy { get; set; }

    }
    public class ProductDataResponseBySubSubCategoryDto
    {
        public string ProductTitle { get; set; }
        public string ProductSKU { get; set; }
        public string ImageUrl { get; set; }

    }
    public class ProductSubSubCategoryDto
    {
        public long CatergoryId { get; set; }
        public string CatergoryName { get; set; }
        public long SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string SubSubCategoryTitle { get; set; }
        public long SubSubcategoryId { get; set; }

        public List<ProductListViewDto> product { get; set; }

    }
}
