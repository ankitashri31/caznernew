using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.Position;
using CaznerMarketplaceBackendApp.Position.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    //[AbpAuthorize]
    public class PositionAppService : AsyncCrudAppService<PositionMaster,PositionDto,long, PositionResultRequestDto, CreateOrUpdatePosition,PositionDto>,IPositionAppService
    {
        public PositionAppService(IRepository<PositionMaster,long> repository) : base(repository)
        {

        }
    }
}
