using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.HexColorCodes.Dto
{
    public class HexColorCodesMasterDto : EntityDto<long>
    {
       
        public string Color { get; set; }
        public string HexCode { get; set; }
        public string ColorFamily { get; set; }
        public bool IsActive { get; set; }

    }
}
