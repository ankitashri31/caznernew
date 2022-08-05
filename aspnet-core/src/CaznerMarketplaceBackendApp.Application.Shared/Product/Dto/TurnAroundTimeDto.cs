using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class TurnAroundTimeDto : EntityDto<long>
    {
        public string Time { get; set; }
        public bool IsActive { get; set; }
    }
}
