using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Category
{
    public class CalculationTypeAttributes : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual CalculationTypes CalculationTypes { get; set; }
        [ForeignKey("CalculationTypes")]
        public long  TypeId { get; set; }
        public virtual CalculationTypeTags CalculationTypeTags { get; set; }
        [ForeignKey("CalculationTypeTags")]
        public long TypeMatchId { get; set; }

        public bool IsAssigned { get; set; }
        public bool IsActive { get; set; }

    }
}
