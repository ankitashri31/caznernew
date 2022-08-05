using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.State.Dto
{
    public class StateDto : EntityDto<long>
    {
        public string StateName { get; set; }
        public long CountryId { get; set; }
        public bool IsActive { get; set; }
    }

    public class StateResultRequestDto :PagedResultRequestDto
    {
        public string StateName { get; set; }
        public long? CountryId { get; set; }
        public string Sorting { get; set; }

    }
}
