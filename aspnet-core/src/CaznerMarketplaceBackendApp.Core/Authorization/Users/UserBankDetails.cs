using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Authorization.Users
{
    public class UserBankDetails : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }

        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string AccountName { get; set; }
        public string BranchAddress { get; set; }
        public virtual UserBusinessSettings UserBusinessSettings { get; set; }
        [ForeignKey("UserBusinessSettings")]
        public long? SettingsId { get; set; }
        public bool IsStripeAccount { get; set; }
        public bool IsPayPalAccount { get; set; }
        public bool IsActive { get; set; }
    }
}
