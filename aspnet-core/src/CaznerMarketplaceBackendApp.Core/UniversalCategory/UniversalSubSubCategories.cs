using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.UniversalCategory
{
    public class UniversalSubSubCategories
    {
        public virtual UniversalSubCategoryMaster UniversalCategoryMaster { get; set; }
        [ForeignKey("UniversalCategoryMaster")]
        public long UniversalCategoryId { get; set; }
        public virtual UniversalSubSubCategoryMaster UniversalSubCategory { get; set; }
        [ForeignKey("UniversalSubCategory")]
        public long UniversalSubSubCategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
