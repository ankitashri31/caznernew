using Abp.Application.Services;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.BulkColorCodes;
using CaznerMarketplaceBackendApp.HexColorCodes.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.HexColorCodes
{
    public class HexColorCodesAppServices:
                AsyncCrudAppService<HexColorCodesMaster, HexColorCodesMasterDto, long, RequestHexColors, CreateOrUpdateHexColors, HexColorCodesMasterDto>, IHexColorCodesAppService

    {
        private readonly IRepository<HexColorCodesMaster, long> _repository;
        public HexColorCodesAppServices(IRepository<HexColorCodesMaster, long> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
