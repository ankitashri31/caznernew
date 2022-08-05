using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.SubCategory.Dto
{
  public  class SubCategoryMasterDto : EntityDto<long>
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
}
