using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ECatalogue.Dto
{
    public class CreateOrUpdateECatalogue
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
}
