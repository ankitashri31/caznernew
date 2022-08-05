using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BannerLogo
{
    public class UserLogoBannerDataDto : EntityDto<long>
    {
        public int TenantId { get; set; }
        public long UserId { get; set; }
        public int ImageType { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public long? PageTypeId { get; set; }
        public bool IsActive { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
    }



    public class UserBannerLogoRequest
    {
        public int ImageType { get; set; }
        public long? PageTypeId { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public string ImagePath { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class UserLogoBannerResultRequestDto : PagedResultRequestDto
    {
        public long Id { get; set; }
        public int ImageType { get; set; }
        public long? PageTypeId { get; set; }
        public string EncryptedTenantId { get; set; }
        public List<UserLogoBannerDataDto> InputData { get; set; }

    }

}
