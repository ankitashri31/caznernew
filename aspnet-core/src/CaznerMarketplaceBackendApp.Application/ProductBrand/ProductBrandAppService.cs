using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.ProductBrand.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.ProductBrand
{
    [AbpAuthorize]
    public class ProductBrandAppService :
        AsyncCrudAppService<ProductBrandMaster, ProductBrandDto, long, ProductBrandResultRequestDto, CreateOrUpdateProductBrand, ProductBrandDto>, IProductBrandAppService
    {
        private readonly IRepository<ProductBrandMaster, long> _repository;
        public ProductBrandAppService(IRepository<ProductBrandMaster, long> repository) : base(repository)
        {
            _repository = repository;
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
        }

        public override async Task<ProductBrandDto> CreateAsync(CreateOrUpdateProductBrand collectionDto)
        {
            var productBrand = ObjectMapper.Map<ProductBrandMaster>(collectionDto);

            var productBrandName = _repository.GetAllList(i => i.ProductBrandName.ToLower().Trim() == collectionDto.ProductBrandName.ToLower().Trim()).FirstOrDefault();
            if (productBrandName == null)
            {
                long Id = await _repository.InsertAndGetIdAsync(productBrand);
                return MapToEntityDto(productBrand);
            }
            else
            {
                throw new UserFriendlyException(L("BrandTitleAlreadyExists"));
            }
            
        }
    }
}
