using Abp.Application.Services;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using CaznerMarketplaceBackendApp.Country.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection
{
    public interface ICategoryAppService : IAsyncCrudAppService<CategoryDto, long, CategoryResultRequestDto, CreateCategoryDto, CategoryDto>
    {
    }
}
