using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Store.Dto
{
    public class StoreMasterDto : EntityDto<long>
    {       
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
        public bool IsActive { get; set; }

        public List<StoreOpeningTimingsDto> StoreOpeningTimings { get; set; }
    }
}
