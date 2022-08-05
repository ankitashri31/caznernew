using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.SubCategory.Dto
{
    public class CreateSubCategoryDto
    {
        public long SubCategoryId { get; set; }
        public string SubCategoryTitle { get; set; }
        public List<long> CategoryIds { get; set; }
        public long CategorySubCategoryId { get; set; }
        public string ImagePath { get; set; }
        public ProductImageType ImageObj { get; set; }


    }
}
