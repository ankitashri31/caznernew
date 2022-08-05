using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Store.Dto
{
    public class StoreOpeningTimingsDto : EntityDto<long>
    {       
        public string WeekDay { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public bool IsCompleteDay { get; set; }
        public bool IsActive { get; set; }
    }
}
