using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class FrontendMediaImagesDto
    {
        public List<ProductImageType> LineArtImages { get; set; }
        public List<ProductImageType> OtherMediaImages { get; set; }
        public List<ProductImageType> LifeStyleImages { get; set; }
        public List<ProductImageType> AllProductImages { get; set; }

    }


    public class ImagesCustomlistDto : PagedResultRequestDto
    {
        //public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<ProductImageType> items { get; set; }
    }
    public class FrontEndMediaFilter : PagedResultRequestDto
    {
        public string EncryptedTenantId { get; set; }
        public string ProductId  { get; set; }
    }

    public class BaseImageFilter
    {
        public string EncryptedTenantId { get; set; }
        public long ProductId { get; set; }
        public int FlagType { get; set; } 
        public bool IsdefaultImage { get; set; } 
    }
}
