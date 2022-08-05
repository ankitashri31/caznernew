using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Department.Dto
{
    public class DepartmentDto : EntityDto<long>
    {        
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
    }
    public class DepartmentResultRequestDto : PagedResultRequestDto
    {
        public string DepartmentName { get; set; }
        public string Sorting { get; set; }

    }
}
