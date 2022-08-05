using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Country.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CaznerMarketplaceBackendApp.AppConsts;

namespace CaznerMarketplaceBackendApp.Country
{
    //[AbpAuthorize]
    public class CountryAppService :
         AsyncCrudAppService<Countries, CountryDto, long, CountryResultRequestDto, CreateOrUpdateCountry, CountryDto>, ICountryAppService
    {
        private readonly IRepository<Countries, long> _repository;
        private readonly IRepository<States, long> _repositoryState;
        private readonly IRepository<UserDetails, long> _repositoryUserdetails;

        public CountryAppService(IRepository<Countries, long> repository, IRepository<States, long> repositoryState, IRepository<UserDetails, long> repositoryUserdetails) : base(repository)
        {
            _repository = repository;
            _repositoryState = repositoryState;
            _repositoryUserdetails = repositoryUserdetails;
        }

        public override async Task<PagedResultDto<CountryDto>> GetAllAsync(CountryResultRequestDto Input)
        {
            int TotalCount = 0;
            IQueryable<CountryDto> model = Enumerable.Empty<CountryDto>().AsQueryable();
            try
            {
                var CountryData = _repository.GetAll()
                      .WhereIf(!string.IsNullOrEmpty(Input.SearchText), x => x.CountryName.ToLower().Trim().Contains(Input.SearchText.ToLower().Trim()))
                      .WhereIf(Input.CountryId.HasValue, x => x.Id == Input.CountryId);

                model = (from country in CountryData.AsQueryable()
                         select new CountryDto
                         {
                             Id = country.Id,
                             CountryName = country.CountryName,
                            // StateName = (from state in _repositoryState.GetAll().Where(i => i.CountryId == country.Id) select state.StateName).ToList(),
                             IsActive = true,
                         });

                TotalCount = model.ToList().Count();
                model = await ApplyDtoSorting(model, Input);

            }
            catch (Exception ex)
            {

            }
            return new PagedResultDto<CountryDto>(TotalCount, ObjectMapper.Map<List<CountryDto>>(model.ToList()));
        }

        public override async Task<CountryDto> CreateAsync(CreateOrUpdateCountry Model)
        {
            Countries countries = new Countries();
            CountryDto Response = new CountryDto();
            try {
                if (Model != null)
                {
                    var isExists = _repository.GetAll().Where(i => i.CountryName.ToLower() == Model.CountryName.ToLower()).FirstOrDefault();
                    if (isExists == null)
                    {
                        countries.CountryName = Model.CountryName;
                        countries.IsActive = Model.IsActive;
                        long id = await _repository.InsertAndGetIdAsync(countries);
                        Response.CountryName = Model.CountryName;
                        Response.IsActive = Model.IsActive;
                        Response.Id = id;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Response;

        }
        public override async Task<CountryDto> UpdateAsync(CountryDto Model) 
        {
            CountryDto Response = new CountryDto();
            try
            {
                if (Model != null)
                {
                    var Countries = _repository.GetAll().Where(i => i.Id == Model.Id).FirstOrDefault();
                    if (Countries != null)
                    {
                        var isDuplicate = _repository.GetAll().Where(i => i.CountryName.ToLower() == Model.CountryName.ToLower() && i.Id != Model.Id).FirstOrDefault();
                        if (isDuplicate == null)
                        {
                            Countries.CountryName = Model.CountryName;
                            Countries.IsActive = Model.IsActive;
                            await _repository.UpdateAsync(Countries);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Model;
        }

        #region sorting 
        private async Task<IQueryable<CountryDto>> ApplyDtoSorting(IQueryable<CountryDto> query, CountryResultRequestDto input)
        {
            if (input.Sorting.HasValue)
            {
                switch (input.Sorting)
                {
                    case 1:// All
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.Id);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Id);
                        }

                        break;
                    case 2:// Relevance
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.CountryName);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CountryName);
                        }
                        break;
                    case 3:// Name
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.CountryName);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CountryName);
                        }
                        break;
                }
            }
            return query;
        }
        #endregion


    }

}
