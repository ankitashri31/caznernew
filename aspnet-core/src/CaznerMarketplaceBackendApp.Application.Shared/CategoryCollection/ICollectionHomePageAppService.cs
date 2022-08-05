using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services;
using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;

namespace CaznerMarketplaceBackendApp.CategoryCollection
{
    public interface ICollectionHomePageAppService : IApplicationService
    {
        Task CreateCollection(CollectionHomePageDto createCollection);
    }
}
