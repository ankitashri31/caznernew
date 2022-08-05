using Abp.Application.Services.Dto;



namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductAssignedMaterialsDto : EntityDto<long>
    {
        public long ProductMaterialId { get; set; }       
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
