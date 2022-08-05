using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Department.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Department
{
    public interface IDepartmentAppService : IAsyncCrudAppService<DepartmentDto, long, DepartmentResultRequestDto, CreateOrUpdateDepartment, DepartmentDto>
    {
       
    }
}
