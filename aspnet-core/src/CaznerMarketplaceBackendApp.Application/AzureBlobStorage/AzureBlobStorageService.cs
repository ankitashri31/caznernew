using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;

namespace CaznerMarketplaceBackendApp.AzureBlobStorage
{
    public class AzureBlobStorageService : CaznerMarketplaceBackendAppAppServiceBase, IAzureBlobStorageService
    {
        public IConfiguration _configuration;
        public CloudBlobContainer _blobContainer;
        public AzureBlobStorageService(IConfiguration configuration) {
            _configuration = configuration;
            string AccountName = _configuration["AzureStorage:AccountName"];
            string AccessKey = _configuration["AzureStorage:AccessKey"];
            StorageCredentials creden = new StorageCredentials(AccountName, AccessKey);
            CloudStorageAccount acc = new CloudStorageAccount(creden, useHttps: true);
            CloudBlobClient client = acc.CreateCloudBlobClient();
            _blobContainer = client.GetContainerReference("orsostorage");
            _blobContainer.CreateIfNotExistsAsync();
            _blobContainer.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
        }
        public async Task<string> SaveBlobImage(byte[] ImageByte, string filePath, string fileName, string ContentType)
        {
            string FileAbsoluteUri = string.Empty;
            try
            {
                var File = Path.Combine(filePath, fileName.Trim('"').Replace(" ", ""));
                CloudBlockBlob CBlob = _blobContainer.GetBlockBlobReference(File);
                CBlob.Properties.ContentType = ContentType;
                if (await CBlob.ExistsAsync())
                {
                    FileAbsoluteUri = "Exists";
                    if (await CBlob.DeleteIfExistsAsync())
                    {
                        FileAbsoluteUri = "Deleted";
                    }
                }

                using (var MS = new MemoryStream(ImageByte, false))
                {
                    await CBlob.UploadFromStreamAsync(MS);
                }
                if (CBlob.Properties.Length >= 0)
                {
                    FileAbsoluteUri = CBlob.Uri.AbsoluteUri;
                }
                else
                { FileAbsoluteUri = "Fail"; }

            }
            catch (Exception ex)
            {
                Logger.Error("ImageHelper => error failed to save image ", ex);
            }

            return FileAbsoluteUri;

        }

        //public async Task<bool> Delete(string filePath, string fileName)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        var path = Path.Combine(filePath, fileName.Trim('"').Replace(" ", ""));
        //        // get block blob refarence
        //        CloudBlockBlob cblob = _blobContainer.GetBlockBlobReference(path);
        //        if (await cblob.ExistsAsync())
        //        {
        //            if (await cblob.DeleteIfExistsAsync())
        //            {
        //                flag = true;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("ImageHelper => error failed to delete image ", ex);
        //    }

        //    return flag;
        //}
    }
}
