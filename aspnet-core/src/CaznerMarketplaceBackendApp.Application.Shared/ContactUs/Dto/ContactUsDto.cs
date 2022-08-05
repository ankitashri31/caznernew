using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ContactUs.Dto
{
    public class ContactUsDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string MessageSubject { get; set; }
        public string MessageBody { get; set; }
        public string PhoneNumber { get; set; }

        public string EncryptedTenantId { get; set; }
        //public bool IsRepliedByAdmin{ get; set; }
        //public bool IsAnswerMailSent{ get; set; }

        //public string AnswerMessageBody { get; set; }
        //public long? AnsweredByUserId { get; set; }
        //public DateTime? AnsweredOn { get; set; }
        public bool IsActive { get; set; }
    }
}
