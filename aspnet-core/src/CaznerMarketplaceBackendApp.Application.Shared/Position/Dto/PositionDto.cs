using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Position.Dto
{
    public class PositionDto : EntityDto<long>
    {
        public string PositionName { get; set; }
        public bool IsActive { get; set; }
    }

    public class PositionResultRequestDto : PagedResultRequestDto
    {
        public string PositionName { get; set; }       

    }
}
