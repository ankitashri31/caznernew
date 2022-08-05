using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.FAQ.Dto
{
    public class AnswerDto : EntityDto<long>
    {
        public long QuestionId { get; set; }
        public string Answer { get; set; }
        public bool IsActive { get; set; }
    }
}
