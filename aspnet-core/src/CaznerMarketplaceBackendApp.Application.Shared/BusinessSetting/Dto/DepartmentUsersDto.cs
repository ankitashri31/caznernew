using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BusinessSetting.Dto
{
    public class DepartmentUsersDto : EntityDto<long>
    {
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
    }
}
