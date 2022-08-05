using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection.Dto
{
    public class CategoryGroupDto
    {
        public long Id { get; set; }
        public string GroupTitle { get; set; }
        public bool IsActive { get; set; }
        public string GroupImageUrl { get; set; }
        public string ImageName { get; set; }
        public int GroupProductsCount { get; set; }
        public List<CategoryGroupModel> CategoryList { get; set; }
        public ProductImageType ImageObj { get; set; }
        public string ImagePath { get; set; }
        public long CategoryMasterId { get; set; }
        public long CategoryGroupId { get; set; }
        public string CategoryTitle { get; set; }
        public string Url { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int SequenceNumber { get; set; }
        public string ValidationMessage { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class GroupRequestDto
    {
        public string EncryptedTenantId { get; set; }
        public int? Sorting { get; set; }
        public int? FilterBy { get; set; }
        public string SearchText { get; set; }
        public bool IsOrderBySeqNumber { get; set; }
        public int MaxResultCount { get; set; }
        public int SkipCount { get; set; }
    }
    public class CategoryGroupDataDto
    {
        public long CategoryId { get; set; }
        public string CategoryTitle { get; set; }

        public bool IsActive { get; set; }
        public string CategoryImageUrl { get; set; }
        public string ImageName { get; set; }
        public List<CategoryAllModel> CategoryList { get; set; }
        public ProductImageType ImageObj { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
    public class AllCategoryTypeData
    {
        public long CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string ImageUrl { get; set; }
        public string Ext { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string SubCategoryTitle { get; set; }
        public long SubCategoryId { get; set; }
        public string SubSubCategoryTitle { get; set; }
        public long SubSubCategoryId { get; set; }
    }
    public class AllCategoryTypeMatserData
    {
        public long Id { get; set; }
        public int SequenceNumber { get; set; }
        public string categoryTitle { get; set; }
        public int[] categorySelectedTitle { get; set; }
        public bool IsActive { get; set; }
        public List<CategoryDataModel> CategoryList { get; set; }
    }
    public class CategoryDataDto
    {
        public long CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public int SequenceNumber { get; set; }
        public bool IsActive { get; set; }
        public string GroupImageUrl { get; set; }
        public long SubCategoryMasterId { get; set; }
        public string SubCategoryImageUrl { get; set; }
        public string SubCategoryTitle { get; set; }
        public DateTime CreationTime { get; set; }
        public string Url { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string SubCategoryUrl { get; set; }
        public string SubCategoryExt { get; set; }
        public string SubCategoryImageSize { get; set; }
        public string SubCategoryImageType { get; set; }
        public string SubcategoryImageName { get; set; }
        public long AssignmentId { get; set; }
    }
    public class CategoryCustomDataDto
    {
        public long CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public bool IsActive { get; set; }
        public string GroupImageUrl { get; set; }
        public DateTime CreationTime { get; set; }
        public List<CategorySubModel> CategoryList { get; set; }
        public ProductImageType ImageObj { get; set; }
        public string ImagePath { get; set; }
        public int SequenceNumber { get; set; }
        public long AssignmentId { get; set; }

    }
    public class CategorySubModel
    {
        public long SubCategoryMasterId { get; set; }
        public string SubCategoryTitle { get; set; }
        public ProductImageType SubCategoryImageObj { get; set; }

    }
}
