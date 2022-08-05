using Abp.Application.Services;
using CaznerMarketplaceBackendApp.HexColorCodes.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.HexColorCodes
{
    public interface IHexColorCodesAppService : IAsyncCrudAppService<HexColorCodesMasterDto, long, RequestHexColors, CreateOrUpdateHexColors, HexColorCodesMasterDto>
    {
    }
}
