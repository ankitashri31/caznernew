using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Masters
{
    public class TurnAroundTime : FullAuditedEntity<long>
    {
        public string Time { get; set; }
        public string NumberOfDays { get; set; }
        public bool IsActive { get; set; }
    }
}
