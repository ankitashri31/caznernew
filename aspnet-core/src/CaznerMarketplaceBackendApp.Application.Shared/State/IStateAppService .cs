using Abp.Application.Services;
using CaznerMarketplaceBackendApp.State.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.State
{
    public interface IStateAppService : IAsyncCrudAppService<StateDto, long, StateResultRequestDto, CreateOrUpdateState, StateDto>
    {
    }
}
