using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.FAQ.Dto
{
    public class QuestionAnswerDataDto 
    {
        public long QuestionId { get; set; }
        public string Question { get; set; }
        public int UserType { get; set; }
        //public bool IsActive { get; set; }
        public long AnswerId { get; set; }
        public string Answer { get; set; }
    }

    //public class QuestionAnswerDetail
    //{
    //    public List<QuestionAnswerDataDto> QuestionAnswerDetails { get; set; }

    //    //public List<AnswerDto> AnswerDetails { get; set; }
    //}
    public class QuestionAnswerModel
    {
        public List<QuestionAnswerDataDto> SupplierModel { get; set; }
        public List<QuestionAnswerDataDto> DistributorModel { get; set; }
    }
}
