using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.Product.Dto;
using CaznerMarketplaceBackendApp.SubCategory;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection.Dto
{
    public class CategoryDto : EntityDto<long>
    {
        public int TenantId { get; set; }
        public string CategoryTitle { get; set; }
        public bool IsActive { get; set; }
        public long CategoryGroupId { get; set; }
        public string Code { get; set; }
        public string CategoryGroupTitle { get; set; }
        public string CategoryImageUrl { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public int CategoryProductsCount { get; set; }
        public long CategorySubCategoryId { get; set; }
        public string SubCategoryTitle { get; set; }

        public long SubCategoryId { get; set; }
        //public List<CollectionModel> Collections { get; set; }
        public List<SubCategoryModel> SubCategory { get; set; }
        public List<CategorySubCategoryModel> CategorySubCategory { get; set; }

        public ProductImageType ImageObj { get; set; }
        public long SubSubCategoryId { get; set; }
        public string SubSubCategoryTitle { get; set; }


        public string Url { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public string ValidationMessage { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class CategoryModel
    {
        public long Id { get; set; }
        public long CategoryCollectionId { get; set; }
        public string CategoryTitle { get; set; }
    }

    public class CategoryGroupModel
    {
        public long Id { get; set; }
        public long CategoryGroupId { get; set; }
        public string CategoryTitle { get; set; }
        public ProductImageType ImageObj { get; set; }

        public List<CategorySubSubModel> SubSubCategory { get; set; }


    }
    public class CategoryDataModel
    {
        public long SubCategoryId { get; set; }
        public string SubCategoryTitle { get; set; }
        public int[] SubCategorySelectedTitle { get; set; }

        public List<SubSubCategoryData> SubSubCategory { get; set; }

    }

    public class SubSubCategoryData
    {

        public long SubSubCategoryId { get; set; }
        public string SubSubCategoryTitle { get; set; }
        public int[] SubSubCategorySelectedTitle { get; set; }


    }

    public class CollectionModel
    {
        public long Id { get; set; }
        public long CategoryCollectionId { get; set; }
        public string CollectionName { get; set; }

        public string Code { get; set; }
    }
    public class SubCategoryModel
    {
        public long Id { get; set; }
        public long CategorySubCategoryId { get; set; }
        public string Title { get; set; }

        public string Code { get; set; }
    }
    public class CategorySubCategoryModel
    {
        public long Id { get; set; }
        public long CategorySubCAtegoryId { get; set; }
        public string Title { get; set; }

        public string Code { get; set; }
    }
    public class CategoryAllModel
    {
        public long SubCategoryId { get; set; }
        public string SubCategoryTitle { get; set; }
        public long SubSubCategoryId { get; set; }

        public string SubSubCategoryTitle { get; set; }
    }
    public class CategoryResultRequestDto : PagedResultRequestDto
    {
        //public string Sorting { get; set; }
        public int? Sorting { get; set; }
        public int? FilterBy { get; set; }
        public long? GroupId { get; set; }
        public string EncryptedTenantId { get; set; }
        public string SearchText { get; set; }

    }
    public class SubCategoryRequestDto : PagedResultRequestDto
    {
        //public string Sorting { get; set; }
        public int? Sorting { get; set; }
        public int? FilterBy { get; set; }
        public long? CategoryId { get; set; }
        public string EncryptedTenantId { get; set; }
        public string SearchText { get; set; }

    }
    public class CategoryCustomlistDto : PagedResultRequestDto
    {
        //public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<CategoryDto> items { get; set; }
    }
    public class SubCategoryDataDto
    {

        public bool IsActive { get; set; }
        public long SubSubCategoryId { get; set; }
        public string SubSubCategoryTitle { get; set; }
        public string SubCategoryTitle { get; set; }
        public string CategoryTitle { get; set; }
        public string ImageUrl { get; set; }
        public string ImageExt { get; set; }
        public string ImageSize { get; set; }
        public string ImageType { get; set; }
        public string ImageName { get; set; }
        public string Url { get; set; }
        public long SubCategoryId { get; set; }
    }
    public class CategorySubSubModel
    {
        public long Id { get; set; }
        public long SubSubCategoryMasterId { get; set; }
        public string SubSubCategoryTitle { get; set; }
        public ProductImageType SubCategoryImageObj { get; set; }
        public List<ProductListDto> ProductList { get; set; }
    }
    public class ProductListDto
    {
        //public int SkipCount { get; set; }
        public long Id { get; set; }
        public string ProductSKU { get; set; }
        public string ProductTitle { get; set; }
        public List<ProductImageType> ImageObj { get; set; }



    }
    public class SubCategoryCustomDataDto
    {
        public long SubCategoryId { get; set; }
        public string SubCategoryTitle { get; set; }
        public string CategoryTitle { get; set; }
        public bool IsActive { get; set; }
        public List<CategorySubSubModel> CategoryList { get; set; }
        public ProductImageType ImageObj { get; set; }
        public string ImagePath { get; set; }

    }
}
