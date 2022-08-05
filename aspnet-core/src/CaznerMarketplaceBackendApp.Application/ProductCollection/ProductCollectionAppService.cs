using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.ProductCollection.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.ProductCollection
{
    [AbpAuthorize]
    public class ProductCollectionAppService :
        AsyncCrudAppService<ProductCollectionMaster, ProductCollectionDto, long, ProductCollectionResultRequestDto, CreateOrUpdateProductCollection, ProductCollectionDto>, IProductCollectionAppService
    {
        private readonly IRepository<ProductCollectionMaster, long> _repository;
        public ProductCollectionAppService(IRepository<ProductCollectionMaster, long> repository) : base(repository)
        {
            _repository = repository;
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
        }

        public override async Task<ProductCollectionDto> CreateAsync(CreateOrUpdateProductCollection collectionDto)
        {
            var productCollection  = ObjectMapper.Map<ProductCollectionMaster>(collectionDto);

            var productCollectionName = _repository.GetAllList(i => i.ProductCollectionName.ToLower().Trim() == collectionDto.ProductCollectionName.ToLower().Trim()).FirstOrDefault();
            if (productCollectionName == null)
            {
                long Id = await _repository.InsertAndGetIdAsync(productCollection);
                return MapToEntityDto(productCollection);
            }
            else
            {
                throw new UserFriendlyException(L("CollectionTitleAlreadyExists"));
            }
        }
    }
}
