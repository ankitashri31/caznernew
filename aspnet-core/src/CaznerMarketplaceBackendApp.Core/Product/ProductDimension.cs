using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductDimension:FullAuditedEntity<long>
    {
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }

        public string Height { get; set; }
        public string Width { get; set; }
        public string Length { get; set; }

        public string UnitWeight { get; set; }
        public string CartonWeight { get; set; }
        public int CartonQuantity { get; set; }
    }
}
