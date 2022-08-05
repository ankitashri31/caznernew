using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.HexColorCodes.Dto
{
    public class CreateOrUpdateHexColors
    {
        public string Color { get; set; }
        public string HexCode { get; set; }
        public string ColorFamily { get; set; }

    }

    public class RequestHexColors : PagedResultRequestDto
    {
        public string Color { get; set; }
     

    }

}
