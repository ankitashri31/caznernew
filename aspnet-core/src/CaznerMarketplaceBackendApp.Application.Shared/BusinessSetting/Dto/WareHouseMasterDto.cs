using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.Country.Dto;
using CaznerMarketplaceBackendApp.State.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BusinessSetting.Dto
{
    public class WareHouseMasterDto: EntityDto<long>
    {
        
        public string WarehouseTitle { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }       
        public StateDto StateId { get; set; }
        public string PostCode { get; set; }        
        public CountryDto CountryId { get; set; }
        public string TimezoneId { get; set; }
        public bool IsActive { get; set; }
    }
}
