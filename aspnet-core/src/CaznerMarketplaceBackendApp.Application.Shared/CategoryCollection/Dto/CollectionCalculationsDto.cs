using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection.Dto
{
    public class CollectionCalculationsDto : EntityDto<long>
    {
        public int TenantId { get; set; }
        public long CollectionId { get; set; }
        public string TypeTitle { get; set; }
        public string TypeMatchTitle { get; set; }
        public long? TypeMatchId { get; set; }
        public long? TypeId { get; set; }
        public string EntityValue { get; set; }
        public bool IsActive { get; set; }
        public TypeModel TypeMatchData { get; set; }
    }

    public class CollectionCalculationsModel
    {
        public int TenantId { get; set; }
        public long CollectionId { get; set; }
        public string TypeTitle { get; set; }
        public string TypeMatchTitle { get; set; }
        public long? TypeMatchId { get; set; }
        public long? TypeId { get; set; }
        public string EntityValue { get; set; }
        public bool IsActive { get; set; }
        public TypeModel TypeMatchData { get; set; }

    }

    public class TypeModel
    {
        public long? Id { get; set; }
        public string TypeMatchTitle { get; set; }
        public List<CalculationTypesDto> TypeList { get; set; }
    }
}
