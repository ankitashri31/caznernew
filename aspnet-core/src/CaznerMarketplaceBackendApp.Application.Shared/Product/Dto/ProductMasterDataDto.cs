using CaznerMarketplaceBackendApp.BrandingMethod.Dto;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using CaznerMarketplaceBackendApp.ProductBrand.Dto;
using CaznerMarketplaceBackendApp.ProductCollection.Dto;
using CaznerMarketplaceBackendApp.ProductMaterial.Dto;
using CaznerMarketplaceBackendApp.ProductSize.Dto;
using CaznerMarketplaceBackendApp.ProductType.Dto;
using CaznerMarketplaceBackendApp.SubCategory;
using CaznerMarketplaceBackendApp.SubCategory.Dto;
using CaznerMarketplaceBackendApp.Tag.Dto;
using CaznerMarketplaceBackendApp.WareHouse.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductMasterDataDto
    {
        public List<ProductBrandDto> ProductBrandData { get; set; }

        public List<ProductTagDto> ProductTagData { get; set; }

        public List<CategoryCollectionsDto> ProductCollectionData { get; set; }

        public List<ProductMaterialDto> ProductMaterialData { get; set; }

        public List<ProductTypeDto> ProductTypeData { get; set; }
        public List<SupplierListDto> SupplierListData { get; set; }

        public List<BrandingMethodDto> BrandingmethodData { get; set; }
        public List<ProductOptionDto> ProductOptionsData { get; set; }
        public List<WareHouseDto> WareHouseData { get; set; }
        public List<TurnAroundTimeDto> TurnAroundTimeData { get; set; }
        public List<CategoryIdDto> CategoryList { get; set; }
        public List<ProductSizeDto> UnitSizeList { get; set; }
        public List<ProductSizeDto> WeightMeasureList { get; set; }
        public List<ProductOptionDto> CompartmentOptionData { get; set; }
        public List<SubCategoryMasterDto> SubCategoryMasterData { get; set; }
    }
}
