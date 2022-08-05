using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection.Dto
{
    public class CalculationTypesDto: EntityDto<long>
    {
        public int TenantId { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAssigned { get; set; }
    }
}
