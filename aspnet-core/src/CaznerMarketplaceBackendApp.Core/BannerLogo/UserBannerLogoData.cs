using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.BannerLogo
{
    public class UserBannerLogoData : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual ImageTypeMaster ImageTypeMaster { get; set; }
        [ForeignKey("ImageTypeMaster")]
        public long ImageTypeId { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public virtual BannerPageTypeMaster BannerPageTypeMaster { get; set; }
        [ForeignKey("BannerPageTypeMaster")]
        public long? PageTypeId { get; set; }
        public bool IsActive { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
