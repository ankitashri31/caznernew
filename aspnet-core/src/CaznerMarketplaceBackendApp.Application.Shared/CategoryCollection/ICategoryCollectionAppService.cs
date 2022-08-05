using Abp.Application.Services;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.CategoryCollection
{
    public interface ICategoryCollectionAppService : IAsyncCrudAppService<CategoryCollectionsDto, long, CategoryCollectionsResultRequestDto, CreateCategoryCollectionDto, CategoryCollectionsDto>
    {
    }
}
