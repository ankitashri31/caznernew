using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.FAQ
{
    public class QuestionsMaster: FullAuditedEntity<long>
    {
        public string Question { get; set; }
        public int UserType { get; set; }
        public bool IsActive { get; set; }  
    }
}
