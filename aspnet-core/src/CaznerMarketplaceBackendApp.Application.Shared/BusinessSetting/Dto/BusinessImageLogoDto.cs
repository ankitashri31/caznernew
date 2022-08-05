using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BusinessSetting.Dto
{
  public   class BusinessImageLogoDto : EntityDto<long>
    {
        public string BusinessLogoUrl { get; set; }
        public string LogoExt { get; set; }
        public string LogoName { get; set; }
        public string LogoSize { get; set; }
        public string LogoType { get; set; }

      
    }
}
