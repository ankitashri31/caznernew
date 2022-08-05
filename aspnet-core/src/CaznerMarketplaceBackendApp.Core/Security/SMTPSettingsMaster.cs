using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace CaznerMarketplaceBackendApp.Security
{
    public class SMTPSettingsMaster : FullAuditedEntity<long>
    {
        public string DefaultFromAddress { get; set; }
        public string DefaultFromDisplayName { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpEnableSsl { get; set; }
        public string SmtpUserName { get; set; }

        public bool IsActive { get; set; }
    }
}
