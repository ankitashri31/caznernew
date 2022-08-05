using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.Storage;
using Microsoft.AspNetCore.Mvc;
using CaznerMarketplaceBackendApp.Web.Core.Controllers;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace CaznerMarketplaceBackendApp.Web.Controllers
{
    [AbpAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBulkUploadController : ProductUploadControllerBase   
    {

        public ProductBulkUploadController(ProductBulkUploadAppService repoProductBulkUploadService, IUnitOfWorkManager unitOfWorkManager, IBinaryObjectManager BinaryObjectManager, BackgroundJobManager backgroundJobManager,  IRepository<BackgroundJobInfo, long> backroundjobWorker, IConfiguration configuration, IWebHostEnvironment Environment)
                                : base(repoProductBulkUploadService, unitOfWorkManager, BinaryObjectManager, backgroundJobManager, backroundjobWorker, configuration, Environment)
        {

        }
    }

}
