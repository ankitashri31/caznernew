using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using CaznerMarketplaceBackendApp.Country;
using CaznerMarketplaceBackendApp.State.Dto;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CaznerMarketplaceBackendApp.State
{
   // [AbpAuthorize]
    public class StateAppService : AsyncCrudAppService<States, StateDto, long, StateResultRequestDto, CreateOrUpdateState, StateDto>, IStateAppService
    {
        public readonly IRepository<States, long> _repository;
        public StateAppService(IRepository<States, long> repository) : base(repository)
        {
            _repository = repository;
        }

        protected override IQueryable<States> CreateFilteredQuery(StateResultRequestDto input)
        {
            try
            {
             
                var StateData =  Repository.GetAll().WhereIf(input.CountryId.HasValue, x => x.CountryId == input.CountryId);
                int TotalCount = StateData.ToList().Count();
                if (input.SkipCount == 0)
                {
                    input.MaxResultCount = TotalCount;
                }
                return StateData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
