using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.Currency;
using CaznerMarketplaceBackendApp.Currency.Dto;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.ProductSize.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.ProductSize
{
    [AbpAuthorize]
    public class ProductSizeAppService :
        AsyncCrudAppService<ProductSizeMaster, ProductSizeDto, long, ProductSizeResultRequestDto, CreateOrUpdateProductSize, ProductSizeDto>, IProductSizeAppService
    {
        private readonly IRepository<CurrencyMaster, long> _currencyRepository;
        private readonly IRepository<ProductSizeMaster, long> _repository;

        public ProductSizeAppService(IRepository<ProductSizeMaster, long> repository, IRepository<CurrencyMaster, long> currencyRepository) : base(repository)
        {
            _currencyRepository = currencyRepository;
            _repository = repository;
        }

        public async Task<CurrencySizeDto> GetCurrencySizeList()
        {
            CurrencySizeDto CurrencySizeList = new CurrencySizeDto();
            var productSizeData =(from productSize in _repository.GetAll()
             select new ProductSizeDto
             {
                 Id = productSize.Id,
                 ProductSizeName = productSize.ProductSizeName,
                 IsActive = productSize.IsActive
             }).ToList();

            var currencyData = (from currency in _currencyRepository.GetAll()
                                   select new CurrencyDto
                                   {
                                       Id = currency.Id,
                                       CurrencyName = currency.CurrencyName,
                                       IsActive = currency.IsActive
                                   }).ToList();
            
            CurrencySizeList.ProductSizeData = productSizeData;
            CurrencySizeList.CurrencyData = currencyData;
            return CurrencySizeList;
        }
    }
}

