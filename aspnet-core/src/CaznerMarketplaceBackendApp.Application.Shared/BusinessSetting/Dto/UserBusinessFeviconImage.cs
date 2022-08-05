using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BusinessSetting.Dto
{
   public  class UserBusinessFeviconImage : EntityDto<long>
    {
        public string FaviconImageName { get; set; }
        public string FaviconExt { get; set; }
        public string FaviconSize { get; set; }
        public string FaviconType { get; set; }
        public string FaviconImageURL { get; set; }
    }
}
