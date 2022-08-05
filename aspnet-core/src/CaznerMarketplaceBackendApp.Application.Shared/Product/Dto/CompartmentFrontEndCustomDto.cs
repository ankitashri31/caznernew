using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
   public class CompartmentFrontEndCustomDto : EntityDto<long>
    {

        public List<CompartmentVariantDataDto> CompartmentVariantDataDto { get; set; }

        public ProductImageType BaseImage { get; set; }

    }

    public class BaseImageArray
    {
        public List<BaseImageData> BaseImageData { get; set; }
        public string[] VariantsBaseImageArray { get; set; }
        public string[] PositionBaseImageArray { get; set; }
    }

    public class BaseImageData
    {
        public List<int> FlagsTypes { get; set; }
        public long ProductId { get; set; }
        public string BaseImageString{ get; set; }
 
    }

}
