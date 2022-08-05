using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.WareHouse.Dto
{
    public class WareHouseDto : EntityDto<long>
    {
        public int TenantId { get; set; }
        public string WarehouseTitle { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public long StateId { get; set; }
        public string PostCode { get; set; }
        public long CountryId { get; set; }
        public long? SettingsId { get; set; }
        public string TimezoneId { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
    }

   
}
