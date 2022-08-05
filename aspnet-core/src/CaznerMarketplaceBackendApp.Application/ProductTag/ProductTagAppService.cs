using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.ProductMaterial.Dto;
using CaznerMarketplaceBackendApp.ProductTag.Dto;
using CaznerMarketplaceBackendApp.Tag.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.ProductTag
{
    [AbpAuthorize]
    public class ProductTagAppService :
          AsyncCrudAppService<ProductTagMaster, ProductTagDto, long, ProductTagResultRequestDto, CreateOrUpdateProductTag, ProductTagDto>, IProductTagAppService
    {
        private readonly IRepository<ProductTagMaster, long> _repository;
        public ProductTagAppService(IRepository<ProductTagMaster, long> repository) : base(repository)
        {
            _repository = repository;
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
        }

        public override async Task<ProductTagDto> CreateAsync(CreateOrUpdateProductTag tagDto)
        {
            var productTag  = ObjectMapper.Map<ProductTagMaster>(tagDto);

            var productTagName = _repository.GetAllList(i => i.ProductTagName.ToLower().Trim() == tagDto.ProductTagName.ToLower().Trim()).FirstOrDefault();
            if (productTagName == null)
            {
                long Id = await _repository.InsertAndGetIdAsync(productTag);
                return MapToEntityDto(productTag);
            }
            else
            {
                throw new UserFriendlyException(L("TagTitleAlreadyExists"));
            }
        }
    }

}
