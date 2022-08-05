using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.FAQ
{
    public class AnswersMaster : FullAuditedEntity<long>
    {
        public virtual QuestionsMaster QuestionsMaster { get; set; }
        [ForeignKey("QuestionsMaster")]
        public long QuestionId { get; set; }
        public string Answer { get; set; }       
        public bool IsActive { get; set; }      
    }
}
