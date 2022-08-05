using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.AzureBlobStorage
{
    public interface IAzureBlobStorageService : IApplicationService
    {
        Task<string> SaveBlobImage(byte[] imageByte, string filePath, string fileName, string ContentType);
    }
}
