using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.ArtWork
{
    public class ArtworkImages: FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }

    public string ImageName { get; set; }
    public virtual ArtWorkMaster ArtWorkMaster { get; set; }

    [ForeignKey("ArtWorkMaster")]
    public long ArtworkId { get; set; }
    public string ImagePath { get; set; }
    public string ImageSize { get; set; }
    public byte[] ImageFileData { get; set; }
    public string ImageExtension { get; set; }
    public bool IsActive { get; set; }
    public string Ext { get; set; }
    public string Size { get; set; }
    public string Type { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
}
}
