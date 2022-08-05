using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.FAQ.Dto
{
    public class QuestionDto : EntityDto<long>
    {
        public string Question { get; set; }
        public int Usertype { get; set; }
        public bool IsActive { get; set; }
    }
}
