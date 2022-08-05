using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using CaznerMarketplaceBackendApp.Category;
using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using System.Linq;

namespace CaznerMarketplaceBackendApp.CategoryCollection
{
    public class CollectionCalculationAppService : CaznerMarketplaceBackendAppAppServiceBase, ICollectionCalculationAppService
    {
        private readonly IRepository<CalculationTypeTags, long> _calculationTypeTagsRepository;
        private readonly IRepository<CalculationTypes, long> _calculationTypesRepository;
        private readonly IRepository<CalculationTypeAttributes, long> _calculationTypeAttributesRepository;
        public CollectionCalculationAppService(IRepository<CalculationTypeTags, long> calculationTypeTagsRepository, IRepository<CalculationTypes, long> calculationTypesRepository, IRepository<CalculationTypeAttributes, long> calculationTypeAttributesRepository)
        {
            _calculationTypeTagsRepository = calculationTypeTagsRepository;
            _calculationTypesRepository = calculationTypesRepository;
            _calculationTypeAttributesRepository = calculationTypeAttributesRepository;

        }

        public async Task<CalculationTypeAndTagsDto> GetCalculationTypeData()
        {
            CalculationTypeAndTagsDto Response = new CalculationTypeAndTagsDto();
            try
            {
                Response.CalculationTypes = (from data in _calculationTypesRepository.GetAllList()
                                             select new CalculationTypesDto
                                             {
                                                 Id = data.Id,
                                                 TypeName = data.Type,
                                                 IsActive = data.IsActive
                                             }).ToList();

                Response.CalculationTypeTags = (from data in _calculationTypeTagsRepository.GetAllList()
                                                select new CalculationTypeTagsDto
                                                {
                                                    Id = data.Id,
                                                    TypeTitle = data.TypeTitle,
                                                    IsActive = data.IsActive,
                                                    TypeList = (from type in _calculationTypesRepository.GetAllList()
                                                                select new CalculationTypesDto
                                                                {
                                                                    Id = type.Id,
                                                                    TypeName = type.Type,
                                                                    IsActive = type.IsActive,
                                                                    IsAssigned = _calculationTypeAttributesRepository.GetAll().Where(i => i.TypeMatchId == data.Id && i.TypeId == type.Id).Select(i => i.Id).FirstOrDefault() > 0 ? true : false

                                                                }).ToList()

                                                }).ToList();

            }
            catch (Exception ex)
            {

            }
            return Response;
        }
    }
}
