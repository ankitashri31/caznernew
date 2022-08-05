using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Position.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Position
{
    public interface IPositionAppService : IAsyncCrudAppService<PositionDto, long, PositionResultRequestDto, CreateOrUpdatePosition, PositionDto>
    {
    }
}
