using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ContactUs
{
    public class ContactUsMaster : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string MessageSubject { get; set; }
        public string MessageBody { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsRepliedByAdmin { get; set; }
        public bool IsAnswerMailSent { get; set; }

        public string AnswerMessageBody { get; set; }
        public long? AnsweredByUserId { get; set; }
        public DateTime? AnsweredOn { get; set; }    
        public bool IsActive { get; set; }
    }
}
