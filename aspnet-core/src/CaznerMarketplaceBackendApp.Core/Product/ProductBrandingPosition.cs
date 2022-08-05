using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductBrandingPosition : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long? ProductId { get; set; }
        public string LayerNumber { get; set; }
        public string LayerTitle { get; set; }
        public double PostionMaxwidth { get; set; }
        public double PostionMaxHeight { get; set; }
        public string ImageName { get; set; }
        public byte[] ImageFileData { get; set; }
        public string ImageFileURL { get; set; }
        public bool IsProductSubmissionDone { get; set; }
        public string BrandingLocationNote { get; set; }

        public virtual ProductSizeMaster SizeMaster { get; set; }
        [ForeignKey("SizeMaster")]
        public long? UnitOfMeasureId { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
