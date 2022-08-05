using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.FAQ.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.FAQ
{
    public class FAQAppService : CaznerMarketplaceBackendAppAppServiceBase, IFAQAppService
    {
        private readonly IRepository<QuestionsMaster, long> _questionsMasterRepository;
        private readonly IRepository<AnswersMaster, long> _answersMasterRepository;

        public FAQAppService
           (IRepository<QuestionsMaster, long> questionsMasterRepository,
           IRepository<AnswersMaster, long> answersMasterRepository)
        {
            _answersMasterRepository = answersMasterRepository;
            _questionsMasterRepository = questionsMasterRepository;
        }

        public async Task<QuestionAnswerModel> GetQuestionAnswersdata()
        {
            QuestionAnswerModel ResponseModel = new QuestionAnswerModel();
            List<QuestionAnswerDataDto> SupplierDetail = new List<QuestionAnswerDataDto>();
            List<QuestionAnswerDataDto> DistributorDetail = new List<QuestionAnswerDataDto>();

            SupplierDetail = (from questionMaster in _questionsMasterRepository.GetAll().Where(i => i.UserType == 1)
                                join answerMaster in _answersMasterRepository.GetAll() on questionMaster.Id equals answerMaster.QuestionId
                                select new QuestionAnswerDataDto
                                {
                                      QuestionId = questionMaster.Id,
                                      Question = questionMaster.Question,
                                      UserType = questionMaster.UserType,
                                      AnswerId = answerMaster.Id,
                                      Answer = answerMaster.Answer,
                                }).ToList();

            DistributorDetail = (from questionMaster in _questionsMasterRepository.GetAll().Where(i => i.UserType == 2)
                                join answerMaster in _answersMasterRepository.GetAll() on questionMaster.Id equals answerMaster.QuestionId
                                select new QuestionAnswerDataDto
                                {
                                    QuestionId = questionMaster.Id,
                                    Question = questionMaster.Question,
                                    UserType = questionMaster.UserType,
                                    Answer = answerMaster.Answer,
                                    AnswerId = answerMaster.Id,
                                }).ToList();

            //SupplierDetail = SupplierData;
            //DistributorDetail = DistributorData;

            ResponseModel.SupplierModel = SupplierDetail;
            ResponseModel.DistributorModel = DistributorDetail;

            return ResponseModel;
        }
    }
}
