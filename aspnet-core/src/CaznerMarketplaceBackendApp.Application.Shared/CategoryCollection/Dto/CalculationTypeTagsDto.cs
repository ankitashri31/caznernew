using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection.Dto
{
    public class CalculationTypeTagsDto : EntityDto<long>
    {
        public int TenantId { get; set; }
        public string TypeTitle { get; set; }
        public bool IsActive { get; set; }

        public List<CalculationTypesDto> TypeList { get; set; }
    }

    public class CalculationTypeAndTagsDto
    {
        public List<CalculationTypeTagsDto> CalculationTypeTags { get; set; }
        public List<CalculationTypesDto> CalculationTypes { get; set; }
    }
}
