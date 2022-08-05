using Abp.Application.Services;
using Abp.Domain.Repositories;

using CaznerMarketplaceBackendApp.Department.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.Department
{
    public class DepartmentAppService : AsyncCrudAppService<DepartmentMaster, DepartmentDto, long, DepartmentResultRequestDto, CreateOrUpdateDepartment, DepartmentDto>, IDepartmentAppService
    {
        private readonly IRepository<DepartmentMaster, long> _repository;
        public DepartmentAppService(IRepository<DepartmentMaster, long> repository) : base(repository)
        {
            _repository = repository;
        }

        public override async Task<DepartmentDto> CreateAsync(CreateOrUpdateDepartment departmentDto)
        {
            DepartmentDto model = new DepartmentDto();
            try
            {
                DepartmentMaster departmentMaster = ObjectMapper.Map<DepartmentMaster>(departmentDto);
                departmentMaster.TenantId = Convert.ToInt32(AbpSession.TenantId);
                var departmentId = await _repository.InsertAndGetIdAsync(departmentMaster);
                model.Id = departmentId;
                model.DepartmentName = departmentDto.DepartmentName;
                model.IsActive = departmentDto.IsActive;
            }
            catch (System.Exception)
            {

                //throw;
            }

            return model;
        }
    }
}
