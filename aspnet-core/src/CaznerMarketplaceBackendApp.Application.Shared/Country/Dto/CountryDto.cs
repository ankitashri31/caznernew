using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Country.Dto
{
    public class CountryDto : EntityDto<long>
    {
        public string CountryName { get; set; }

        public bool IsActive { get; set; }
    }

    public class CountryResultRequestDto : PagedResultRequestDto
    {
        public string CountryName { get; set; }
        public int? Sorting { get; set; }
        public int? FilterBy { get; set; }
        public string SearchText { get; set; }
        public int? CountryId { get; set; }

    }
}
