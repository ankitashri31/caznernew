using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ECatalogue.Dto
{
    public class ECatalogueDto : EntityDto<long>
    {
        public int TenantId { get; set; }
        public string Title { get; set; }
        public string CatalogueUrl { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public ProductMediaType ImageObj { get; set; }
    }

    public class ECatalogueRequestDto : PagedResultRequestDto
    {
        public string TiTle { get; set; }
        public int? Sorting { get; set; }
        public int? FilterBy { get; set; }
        public string SearchText { get; set; }
        public string EncryptedTenantId { get; set; }

    }

    public class CustomECatalogueDto : PagedResultRequestDto
    {
        //public int SkipCount { get; set; }
        public bool IsLoadMore { get; set; }
        public int TotalCount { get; set; }
        public List<ECatalogueDto> items { get; set; }
    }
}
