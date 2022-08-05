using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection.Dto
{
    public class CategoryCollectionsDto : EntityDto<long>
    {
        public string Code { get; set; }
        public string CollectionName { get; set; }

        public string ImagePath { get; set; }
        public ProductImageType ImageObj { get; set; }

        public bool IsActive { get; set; }
        public long CollectionId { get; set; }
        public List<CollectionCalculationsDto> Calculations { get; set; }

        public bool IsManualCalculation { get; set; }
        public bool IsMatchAnyCondition { get; set; }

        public bool IsSeoEnabled { get; set; }
        public string SeoPageTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoUrl { get; set; }
        public string Url { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public string ValidationMessage { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class CategoryCollectionsResultRequestDto : PagedResultRequestDto
    {
        public long CollectionId { get; set; }
        public int? Sorting { get; set; }
        public int? FilterBy { get; set; }
        public string SearchText { get; set; }
        public string EncryptedTenantId { get; set; }

    }

    public class CategoryCollectionsModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string CollectionName { get; set; }
        public long CategoryId { get; set; }
        public List<long> CategoryIds { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public bool IsActive { get; set; }
    }


    public class CategoryCollectionsCustomlistDto : PagedResultRequestDto
    {
        //public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public CategoryCollectionsDto items { get; set; }
    }
    public class CategoryCollectionsCustomDto : PagedResultRequestDto
    {
        //public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<CategoryCollectionsDto> items { get; set; }
    }
    public class ProductCollectionListDto
    {
        //public int SkipCount { get; set; }
        public long Id { get; set; }
        public string ProductSKU { get; set; }
        public string ProductTitle { get; set; }
        public string DefaultImagePath { get; set; }
        public long CollectionId { get; set; }
        public string CollectionName { get; set; }
        public int TotalCount { get; set; }


    }
    public class ProductColListDto
    {
        //public int SkipCount { get; set; }
        public long Id { get; set; }
        public string ProductSKU { get; set; }
        public string ProductTitle { get; set; }
        public string DefaultImagePath { get; set; }



    }
    public class CollectionPaginationDto : PagedResultRequestDto
    {
        //public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public List<ProductCollectionDataDto> items { get; set; }
    }
    public class ProductCollectionDataDto
    {
        public long CollectionId { get; set; }
        public string CollectionName { get; set; }
        public int TotalCount { get; set; } 
        public List<ProductColListDto> List { get; set; }
    }

    public class ProductCollectionModel
    {
        public long CollectionId { get; set; }

        public List<long> ProductIds { get; set; }

    }
    public class CategoryProductCollectionsResultRequestDto : PagedResultRequestDto
    {
        public long CollectionId { get; set; }
        public int? Sorting { get; set; }
        public int? FilterBy { get; set; }
        public string SearchText { get; set; }
        public string EncryptedTenantId { get; set; }
        public int PageNumber { get; set; }

    }
}
