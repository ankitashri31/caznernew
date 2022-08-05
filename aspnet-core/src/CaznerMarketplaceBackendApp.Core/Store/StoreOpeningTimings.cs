using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Store
{
    public class StoreOpeningTimings:FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual StoreMaster StoreMaster { get; set; }
        [ForeignKey("StoreMaster")]
        public long StoreMasterId { get; set; }
        public string WeekDay { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public bool IsCompleteDay { get; set; }
        public bool IsActive { get; set; }
    }
}
