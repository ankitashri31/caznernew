using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Tag.Dto
{
    public class ProductTagDto : EntityDto<long>
    {
        public string ProductTagName { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public long AssignmentId { get; set; }
    }
    public class ProductTagResultRequestDto : PagedResultRequestDto
    {
        public string ProductTagName { get; set; }
        public string Sorting { get; set; }

    }
}
