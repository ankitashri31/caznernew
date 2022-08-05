using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.Product.Dto;
using System.Collections.Generic;

public class CompartmentVariantDataDto : EntityDto<long>
{

    public long ProductId { get; set; }
    public string CompartmentTitle { get; set; }
    public string CompartmentSubTitle { get; set; }
    public string ProductName { get; set; }
    public string ProductTitle { get; set; }
    public string SKU { get; set; }
    public string selectedCompartmentVariantImg { get; set; }
    public List<VarientList> VarientList { get; set; }

}
public class VarientList
{
    public long CompartmentId { get; set; }
    public long Id { get; set; }
    public decimal Price { get; set; }
    public ProductImageType ImageObj { get; set; }
    public string SKU { get; set; }
    public long? ProductVarientId { get; set; }

    public string Variant { get; set; }
    public string Color { get; set; }
}