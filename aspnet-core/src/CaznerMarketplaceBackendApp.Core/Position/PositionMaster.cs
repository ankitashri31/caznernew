using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Position
{
    public class PositionMaster : FullAuditedEntity<long>
    {
        public string PositionName { get; set; }
        public bool IsActive { get; set; }
    }
}
