using Abp.Application.Services.Dto;


namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductAssignedCollectionsDto : EntityDto<long>
    {
        public long[] ProductCollectionId { get; set; }      
        public long ProductId { get; set; }
        public bool IsActive { get; set; }

    }
}
