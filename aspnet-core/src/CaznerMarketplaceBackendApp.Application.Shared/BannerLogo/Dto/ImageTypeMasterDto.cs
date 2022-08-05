using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BannerLogo.Dto
{
    public class ImageTypeMasterDto : EntityDto<long>
    {
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
    public class ImageTypeMasterResultRequestDto : PagedResultRequestDto
    {
        public string Title { get; set; }
        public string Sorting { get; set; }

    }
}
