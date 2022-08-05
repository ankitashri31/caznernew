using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace CaznerMarketplaceBackendApp.SubCategory
{
    public class CategorySubCategories : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual CategoryMaster CategoryMaster { get; set; }
        [ForeignKey("CategoryMaster")]
        public long CategoryId { get; set; }
        public virtual SubCategoryMaster SubCategoryMaster { get; set; }
        [ForeignKey("SubCategoryMaster")]
        public long SubCategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
