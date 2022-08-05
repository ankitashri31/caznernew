using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.ProductType.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.ProductType
{
    [AbpAuthorize]
    public class ProductTypeAppService:
        AsyncCrudAppService<ProductTypeMaster, ProductTypeDto, long, ProductTypeResultRequestDto, CreateOrUpdateProductType, ProductTypeDto>, IProductTypeAppService

    {
        private readonly IRepository<ProductTypeMaster, long> _repository;
        public ProductTypeAppService(IRepository<ProductTypeMaster, long> repository) : base(repository)
        {
            _repository = repository;
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
        }

        public override async Task<ProductTypeDto> CreateAsync(CreateOrUpdateProductType typeDto)
        {
            var productType = ObjectMapper.Map<ProductTypeMaster>(typeDto);
           
            var productTypeName = _repository.GetAllList(i => i.ProductTypeName.ToLower().Trim() == typeDto.ProductTypeName.ToLower().Trim()).FirstOrDefault();
            if (productTypeName == null)
            {
                long Id = await _repository.InsertAndGetIdAsync(productType);
                return MapToEntityDto(productType);
            }
            else
            {
                throw new UserFriendlyException(L("TypeTitleAlreadyExists"));
            }
        }
    }
}
