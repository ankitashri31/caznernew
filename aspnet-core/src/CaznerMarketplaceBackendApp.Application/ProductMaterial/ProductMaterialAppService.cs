using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.ProductMaterial.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.ProductMaterial
{
    [AbpAuthorize]
    public class ProductMaterialAppService :
        AsyncCrudAppService<ProductMaterialMaster, ProductMaterialDto, long, ProductMaterialResultRequestDto, CreateOrUpdateProductMaterial, ProductMaterialDto>, IProductMaterialAppService
    {
        private readonly IRepository<ProductMaterialMaster, long> _repository;
        public ProductMaterialAppService(IRepository<ProductMaterialMaster, long> repository) : base(repository)
        {
            _repository = repository;
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
        }

        public override async Task<ProductMaterialDto> CreateAsync(CreateOrUpdateProductMaterial materialDto)
        {
            var productMaterial = ObjectMapper.Map<ProductMaterialMaster>(materialDto);

            var productMaterialName = _repository.GetAllList(i => i.ProductMaterialName.ToLower().Trim() == materialDto.ProductMaterialName.ToLower().Trim()).FirstOrDefault();
            if (productMaterialName == null)
            {
                long Id = await _repository.InsertAndGetIdAsync(productMaterial);
                return MapToEntityDto(productMaterial);
            }
            else
            {
                throw new UserFriendlyException(L("MaterialTitleAlreadyExists"));
            }
            
        }
    }
}
