using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.UniversalCategory
{
    public class UniversalCategorySubcategories
    {
        public virtual UniversalCategoryMaster UniversalCategoryMaster { get; set; }
        [ForeignKey("UniversalCategoryMaster")]
        public long UniversalCategoryId { get; set; }

        public virtual UniversalSubCategoryMaster UniversalSubCategory { get; set; }
        [ForeignKey("UniversalSubCategory")]
        public long UniversalSubCategoryId { get; set; }

        public bool IsActive { get; set; }
    }
}
