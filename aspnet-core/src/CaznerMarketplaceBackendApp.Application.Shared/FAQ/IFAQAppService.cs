using Abp.Application.Services;
using CaznerMarketplaceBackendApp.FAQ.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.FAQ
{
    public interface IFAQAppService : IApplicationService
    {
        Task<QuestionAnswerModel> GetQuestionAnswersdata();
    }
}
