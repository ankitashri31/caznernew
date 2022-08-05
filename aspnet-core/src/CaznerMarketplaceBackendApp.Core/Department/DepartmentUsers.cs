using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Department;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Department
{
    public class DepartmentUsers : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual DepartmentMaster DepartmentMaster { get; set; }
        [ForeignKey("DepartmentMaster")]
        public long DepartmentMasterId { get; set; }
        public virtual UserBusinessSettings UserBusinessSettings { get; set; }
        [ForeignKey("UserBusinessSettings")]
        public long? SettingsId { get; set; }

        public bool IsActive { get; set; }

    }
}
