using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection.Dto
{
    public class CollectionHomePageDto 
    {
        public int TenantId { get; set; }        
        public long CollectionId { get; set; }
        public int NumberOfProducts { get; set; }
        public int SequenceNumber { get; set; }
        public string VideoUrl { get; set; }
        public string VideoFileName { get; set; }
        public bool IsActive { get; set; }
        public string EncryptedTenantId { get; set; }
    }
    public class CollectionRequestDto
    {
       
        public long CollectionId { get; set; }
        public string EncryptedTenantId { get; set; }
    }
    public class CollectionResponseDto
    {
        public long Id { get; set; }
        public string ProductSKU { get; set; }
        public string ProductTitle { get; set; }
        public string DefaultImagePath { get; set; }
        public long CollectionId { get; set; }
        public string CollectionName { get; set; }
        public string VideoUrl { get; set; }

    }
    public class CollectionRibbonResponseDto
    {
        public long Id { get; set; }
        public string CollectionName { get; set; }

    }
    public class CollectionResponseProductDto
    {
       
        public List<ProductColListDto> ProductList { get; set; }
        public long CollectionId { get; set; }
        public string CollectionName { get; set; }
        public string VideoUrl { get; set; }

    }
}
