using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductDetailDto : EntityDto<long>
    {
        public long[] ProductTypeArray { get; set; }
        public long[] ProductBrandArray { get; set; }
        public List<AllCategoryIdDto> ProductCategories { get; set; }
        public long[] ProductCollectionArray { get; set; }
        public long[] ProductMaterialArray { get; set; }
        public long[] ProductTagArray { get; set; }
        public long[] ProductVendorArray { get; set; }
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductDetailComboDto
    {
        public long Id { get; set; }

        public ProductDetailCombo[] ProductTypeArray { get; set; }
        public ProductDetailCombo[] ProductBrandArray { get; set; }
        public ProductDetailCombo ProductCategoriesArray { get; set; }
        public ProductDetailCombo[] ProductCollectionArray { get; set; }
        public ProductDetailCombo[] ProductMaterialArray { get; set; }
        public ProductDetailCombo[] ProductTagArray { get; set; }
        public ProductDetailCombo[] ProductVendorArray { get; set; }
        public ProductDetailCombo[] CollectionListByCategoryId { get; set; }
        public List<AllCategoryIdDto> ProductCategories { get; set; }
        public List<CategoryIdDto> ProductCategoriesData { get; set; }

        public long CategoryId { get; set; }
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductDetailFrontendComboDto
    {
        public long Id { get; set; }
        public string TypeStringArray { get; set; }
        public string BrandStringArray { get; set; }
        public string CollectionStringArray { get; set; }
        public string MaterialStringArray { get; set; }
        public string TagStringArray { get; set; }
        public string VendorStringArray { get; set; }
        public ProductDetailCombo[] ProductTypeArray { get; set; }
        public ProductDetailCombo[] ProductBrandArray { get; set; }
        public ProductDetailCombo[] ProductCollectionArray { get; set; }
        public ProductDetailCombo[] ProductMaterialArray { get; set; }
        public ProductDetailCombo[] ProductTagArray { get; set; }
        public ProductDetailCombo[] ProductVendorArray { get; set; }
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }
    public class AllCategoryIdDto
    {
        public long CategoryId { get; set; }
        public string code { get; set; }
        public long AssignmentId { get; set; }
        public long ProductId { get; set; }
        public int TenantId { get; set; }

        public bool IsActive { get; set; }
        public List<AllSubCategoryIdDto> ProductSubCategories { get; set; }

    }
    public class CategoryIdDto
    {
        public long Id { get; set; }
        public string CategoryTitle { get; set; }
        public long AssignmentId { get; set; }
        public long ProductId { get; set; }
        public int TenantId { get; set; }
        public string[] categorySelectedTitle { get; set; }
        public bool IsActive { get; set; }
        public List<SubCategoryIdDto> CategoryList { get; set; }
        public List<SubCategoryIdDto> subCategorySelectedTitle { get; set; }
        public int SubCount { get; set; }

    }

    public class AllSubCategoryIdDto
    {
        public long SubCategoryId { get; set; }

        public string code { get; set; }
        public long AssignmentId { get; set; }
        public List<AllSubSubCategoryIdDto> ProductSubSubCategories { get; set; }

    }
    public class SubCategoryIdDto
    {
        public long SubCategoryId { get; set; }

        public string SubCategoryTitle { get; set; }
        public long AssignmentId { get; set; }
        public List<SubSubCategoryIdDto> SubsubCategorySelectedTitle { get; set; }
        public List<SubSubCategoryIdDto> subSubCategory { get; set; }
        public int SubSubCount { get; set; }

    }
    public class AllSubSubCategoryIdDto
    {
        public long SubSubCategoryId { get; set; }
        public string code { get; set; }
        public long AssignmentId { get; set; }

    }
    public class SubSubCategoryIdDto
    {
        public long SubSubCategoryId { get; set; }
        public string SubSubCategoryTitle { get; set; }
        public List<SubSubCategoryIdDto> subSubCategorySelectedTitle { get; set; }
        public long AssignmentId { get; set; }

    }

}
