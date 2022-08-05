using Abp.Application.Services;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.CategoryCollection
{
    public interface ICategoryHomePageAppService : IApplicationService
    {
        Task<CategoryHomePageDto> CreateCategoryHomePage(CategoryHomePageDto createCategory);
    }
}
