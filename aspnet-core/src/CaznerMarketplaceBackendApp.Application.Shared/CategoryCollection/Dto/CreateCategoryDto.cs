using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection.Dto
{
    public class CreateCategoryDto
    {
        public string CategoryTitle { get; set; }
        public long CategoryGroupId { get; set; }
        public bool IsActive { get; set; }

        public string CategoryImageName { get; set; }
        // public List<long> Collections { get; set; }
        public ProductImageType ImageObj { get; set; }
    }
}
