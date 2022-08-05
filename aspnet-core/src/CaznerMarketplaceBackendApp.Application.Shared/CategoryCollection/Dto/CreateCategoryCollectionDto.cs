using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection.Dto
{
    public class CreateCategoryCollectionDto
    {
        public string CollectionName { get; set; }
        public long CategoryId { get; set; }
        public List<long> CategoryIds { get; set; }
        public bool IsActive { get; set; }
        public ProductImageType ImageObj { get; set; }
        public List<CollectionCalculationsDto> CalculationsDto { get; set; }
        public bool IsManualCalculation { get; set; }
        public bool IsMatchAnyCondition { get; set; }

        public bool IsSeoEnabled { get; set; }
        public string SeoPageTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoUrl { get; set; }

    }
}
