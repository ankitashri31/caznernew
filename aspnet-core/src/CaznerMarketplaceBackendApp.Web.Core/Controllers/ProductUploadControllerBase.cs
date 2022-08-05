using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.IO.Extensions;
using Abp.UI;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.Storage;
using Microsoft.AspNetCore.Http;
using CaznerMarketplaceBackendApp.Product.Importing;
using CaznerMarketplaceBackendApp.Product.Importing.Dto;
using CaznerMarketplaceBackendApp.Web.Controllers;
using Abp.Runtime.Session;
using Abp.Threading;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Hangfire.Server;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.OleDb;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.Cors;
using CaznerMarketplaceBackendApp.BulkStatesRawImport.Dto;

namespace CaznerMarketplaceBackendApp.Web.Core.Controllers
{
    public abstract class ProductUploadControllerBase : CaznerMarketplaceBackendAppControllerBase
    {
        private readonly ProductBulkUploadAppService _repoProductBulkUploadService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        protected readonly IBinaryObjectManager _BinaryObjectManager;
        protected readonly IBackgroundJobManager _BackgroundJobManager;
        private readonly IRepository<BackgroundJobInfo, long> _backroundjobWorker;
        private IConfiguration _configuration;
        string DBConnection = string.Empty;
        private readonly IWebHostEnvironment _Environment;
        protected ProductUploadControllerBase(ProductBulkUploadAppService repoProductBulkUploadService, IUnitOfWorkManager unitOfWorkManager, IBinaryObjectManager BinaryObjectManager, IBackgroundJobManager BackgroundJobManager, IRepository<BackgroundJobInfo, long> backroundjobWorker, IConfiguration configuration, IWebHostEnvironment Environment)
        {
            _repoProductBulkUploadService = repoProductBulkUploadService;
            _unitOfWorkManager = unitOfWorkManager;
            _BinaryObjectManager = BinaryObjectManager;
            _BackgroundJobManager = BackgroundJobManager;
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
            _backroundjobWorker = backroundjobWorker;
            _configuration = configuration;
            _Environment = Environment;
            DBConnection = _configuration["ConnectionStrings:Default"];
        }                                          


        [HttpPost]
        [Route("UploadBulkProducts")]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<string> UploadBulkProducts(IFormFile Input)
        {
            
            string Response = string.Empty;
            _repoProductBulkUploadService.CreateErrorLogsWithException("Line no 46", "UploadBulkProducts executed", "");
            try
            {
                if (Input == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (Input.Length > 1048576 * 50) //100 MB
                {
                    throw new UserFriendlyException(L("Product_SizeLimit_Error"));
                }

                if (Input.ContentType != "text/csv" && Input.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && Input.ContentType != "application/vnd.ms-excel" && !(Input.ContentType == "application/octet-stream" && Input.FileName.EndsWith(".xlsx"))) //100 MB
                {
                    throw new UserFriendlyException("Unsupported file type. Please download sample file and try again.");
                }

                byte[] fileBytes;
                using (var stream = Input.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                // fileBytes = Input.FileData;
                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    await _BinaryObjectManager.SaveAsync(fileObject);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }
                string BackroundJobId = string.Empty;
                _repoProductBulkUploadService.CreateErrorLogsWithException("Line no 79", "ImportProductsFromExcelJob executed", "");

                string jobId = string.Empty;

                #region store physical file into location and access via db table

                string FilePath =  _repoProductBulkUploadService.SaveFileInWWWRootLocation(fileBytes, Input.FileName, _Environment.WebRootPath + "\\BulkImportFiles");

                //string path = "C:\\15000Data.xlsx";
                string excelCS = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";

              
                    using (OleDbConnection con = new OleDbConnection(excelCS))
                    {
                        OleDbCommand cmd = new OleDbCommand("select * from [Sheet1$]", con);
                        con.Open();
                        // Create DbDataReader to Data Worksheet
                        DbDataReader dr = cmd.ExecuteReader();
                      
                        // SQL Server Connection String
                        // Bulk Copy to SQL Server
                        SqlBulkCopy bulkInsert = new SqlBulkCopy(DBConnection);
                        bulkInsert.BulkCopyTimeout = 0;
                        bulkInsert.DestinationTableName = "ProductBulkImportRawData";
                        bulkInsert.WriteToServer(dr);
                        con.Close();
                    }
                

                #endregion

                jobId = await _BackgroundJobManager.EnqueueAsync<ImportProductsFromExcelJob, ImportProductFromExcelJobArgs>(new ImportProductFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    BackroundJobId = jobId
                }, BackgroundJobPriority.Normal/*, TimeSpan.FromDays(12)*/);

                _repoProductBulkUploadService.CreateErrorLogsWithException("Line no 79", "ImportProductsFromExcelJob response done", "");

                Response = "Job enqued successfully!";
                //Response.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                string exception = ex.StackTrace + "________________________________" + ex.Message;
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                _repoProductBulkUploadService.CreateErrorLogsWithException(line.ToString(), ex.Message, ex.StackTrace);
                throw new UserFriendlyException(ex.StackTrace);
            }
            return Response;
        }


        [HttpPost]
        [Route("UploadBulkColorsCode")]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<string> UploadBulkColorsCode(IFormFile Input)
        {

            string Response = string.Empty;
            
            try
            {
                if (Input == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (Input.Length > 1048576 * 50) //100 MB
                {
                    throw new UserFriendlyException(L("Product_SizeLimit_Error"));
                }

                if (Input.ContentType != "text/csv" && Input.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && Input.ContentType != "application/vnd.ms-excel" && !(Input.ContentType == "application/octet-stream" && Input.FileName.EndsWith(".xlsx"))) //100 MB
                {
                    throw new UserFriendlyException("Unsupported file type. Please download sample file and try again.");
                }

                byte[] fileBytes;
                using (var stream = Input.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                // fileBytes = Input.FileData;
                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    await _BinaryObjectManager.SaveAsync(fileObject);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }

                #region store physical file into location and access via db table

                string FilePath = _repoProductBulkUploadService.SaveFileInWWWRootLocation(fileBytes, Input.FileName, _Environment.WebRootPath + "\\BulkImportFiles");

                //string path = "C:\\15000Data.xlsx";
                string excelCS = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";


                using (OleDbConnection con = new OleDbConnection(excelCS))
                {
                    OleDbCommand cmd = new OleDbCommand("select ColorFamily,Color,HexCode from [Sheet1$]", con);
                    con.Open();
                    // Create DbDataReader to Data Worksheet
                    DbDataReader dr = cmd.ExecuteReader();

                    // SQL Server Connection String
                    // Bulk Copy to SQL Server
                    SqlBulkCopy bulkInsert = new SqlBulkCopy(DBConnection);
                    bulkInsert.ColumnMappings.Add("Color", "Color");
                    bulkInsert.ColumnMappings.Add("HexCode", "HexCode");
                    bulkInsert.ColumnMappings.Add("ColorFamily", "ColorFamily");
                    bulkInsert.BulkCopyTimeout = 0;
                    bulkInsert.DestinationTableName = "HexColorCodesRawData";
                    try
                    {
                        bulkInsert.WriteToServer(dr);
                    }
                    catch(Exception ex)
                    {

                    }
                    con.Close();
                }


                #endregion

               await _BackgroundJobManager.EnqueueAsync<ImportHexColorsFromExcel, ImportBulkColorCodesJobArgs>(new ImportBulkColorCodesJobArgs
               {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                }, BackgroundJobPriority.Normal);

                _repoProductBulkUploadService.CreateErrorLogsWithException("Line no 79", "Bulk Hex Colors dat import done", "");

                Response = "Job enqued successfully!";
                //Response.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                string exception = ex.StackTrace + "________________________________" + ex.Message;
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                _repoProductBulkUploadService.CreateErrorLogsWithException(line.ToString(), ex.Message, ex.StackTrace);
                throw new UserFriendlyException(ex.StackTrace);
            }
            return Response;
        }


        [HttpPost]
        [Route("UploadStatesList")]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<string> UploadStatesList(IFormFile Input)
        {

            string Response = string.Empty;

            try
            {
                if (Input == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (Input.Length > 1048576 * 50) //100 MB
                {
                    throw new UserFriendlyException(L("Product_SizeLimit_Error"));
                }

                if (Input.ContentType != "text/csv" && Input.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && Input.ContentType != "application/vnd.ms-excel" && !(Input.ContentType == "application/octet-stream" && Input.FileName.EndsWith(".xlsx"))) //100 MB
                {
                    throw new UserFriendlyException("Unsupported file type. Please download sample file and try again.");
                }

                byte[] fileBytes;
                using (var stream = Input.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                // fileBytes = Input.FileData;
                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    await _BinaryObjectManager.SaveAsync(fileObject);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }

                #region store physical file into location and access via db table

                string FilePath = _repoProductBulkUploadService.SaveFileInWWWRootLocation(fileBytes, Input.FileName, _Environment.WebRootPath + "\\BulkImportFiles");

                //string path = "C:\\15000Data.xlsx";
                string excelCS = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";


                using (OleDbConnection con = new OleDbConnection(excelCS))
                {
                    OleDbCommand cmd = new OleDbCommand("select CountryISOCode,CountryName,StateName from [Sheet1$]", con);
                    con.Open();
                    // Create DbDataReader to Data Worksheet
                    DbDataReader dr = cmd.ExecuteReader();

                    // SQL Server Connection String
                    // Bulk Copy to SQL Server
                    SqlBulkCopy bulkInsert = new SqlBulkCopy(DBConnection);
                    bulkInsert.ColumnMappings.Add("CountryISOCode", "CountryISOCode");
                    bulkInsert.ColumnMappings.Add("CountryName", "CountryName");
                    bulkInsert.ColumnMappings.Add("StateName", "StateName");
                    bulkInsert.BulkCopyTimeout = 0;
                    bulkInsert.DestinationTableName = "StatesTempRawData";
                    try
                    {
                        bulkInsert.WriteToServer(dr);
                    }
                    catch (Exception ex)
                    {

                    }
                    con.Close();
                }


                #endregion

                await _BackgroundJobManager.EnqueueAsync<ImportStatesFromExcel, ImportStatesJobArgs>(new ImportStatesJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                }, BackgroundJobPriority.Normal);

                _repoProductBulkUploadService.CreateErrorLogsWithException("Line no 79", "Bulk Hex Colors dat import done", "");

                Response = "Job enqued successfully!";
                //Response.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                string exception = ex.StackTrace + "________________________________" + ex.Message;
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                _repoProductBulkUploadService.CreateErrorLogsWithException(line.ToString(), ex.Message, ex.StackTrace);
                throw new UserFriendlyException(ex.StackTrace);
            }
            return Response;
        }


        public class RequestUpload
        {
            public byte[] FileData { get; set; }
            public string FileName { get; set; }
        }
   

        [HttpPost]
        [Route("UploadBulkProductsUsingPath")]
        public async Task<string> UploadBulkProductsUsingPath()
        {
            string Response = string.Empty;
            // var MetadataFile = Request.Form.Files.First();
            try
            {

                var tenantId = AbpSession.TenantId;

                await _repoProductBulkUploadService.ProductImportUsingPath();
                Response = "Job enqued successfully!";

            }
            catch (Exception ex)
            {
                string exception = ex.StackTrace + "________________________________" + ex.Message;
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                _repoProductBulkUploadService.CreateErrorLogsWithException(line.ToString(), ex.Message, ex.StackTrace);
                throw new UserFriendlyException(ex.StackTrace);
            }
            return Response;
        }






        [HttpPost]
        [Route("UploadProductVariants")]
        public async Task UploadProductVariants(IFormFile FileObj)
        {
           
            // var MetadataFile = Request.Form.Files.First();
            try
            {
                if (FileObj == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (FileObj.Length > 1048576 * 50) //100 MB
                {
                    throw new UserFriendlyException(L("Product_SizeLimit_Error"));
                }

                if (FileObj.ContentType != "text/csv" && FileObj.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && FileObj.ContentType != "application/vnd.ms-excel" && !(FileObj.ContentType == "application/octet-stream" && FileObj.FileName.EndsWith(".xlsx"))) //100 MB
                {
                    throw new UserFriendlyException("Unsupported file type. Please download sample file and try again.");
                }

                byte[] fileBytes;
                using (var stream = FileObj.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    await _BinaryObjectManager.SaveAsync(fileObject);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }

                await _repoProductBulkUploadService.ProductVariantImport(fileObject.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("SomethingWentWrongWhileUpload"));
            }

        }


        [HttpPost]
        [Route("UploadProductStockLocationData")]
        public async Task UploadProductStockLocationData(IFormFile FileObj)
        {

            // var MetadataFile = Request.Form.Files.First();
            try
            {
                if (FileObj == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (FileObj.Length > 1048576 * 50) //100 MB
                {
                    throw new UserFriendlyException(L("Product_SizeLimit_Error"));
                }

                if (FileObj.ContentType != "text/csv" && FileObj.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && FileObj.ContentType != "application/vnd.ms-excel" && !(FileObj.ContentType == "application/octet-stream" && FileObj.FileName.EndsWith(".xlsx"))) //100 MB
                {
                    throw new UserFriendlyException("Unsupported file type. Please download sample file and try again.");
                }

                byte[] fileBytes;
                using (var stream = FileObj.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    await _BinaryObjectManager.SaveAsync(fileObject);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }

                await _repoProductBulkUploadService.ProductStockLocationImport(fileObject.Id);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(L("SomethingWentWrongWhileUpload"));
            }

        }

        [HttpPost]
        [Route("UploadProductBrandingPositionData")]
        public async Task UploadProductBrandingPositionData(IFormFile FileObj)
        {

            // var MetadataFile = Request.Form.Files.First();
            try
            {
                if (FileObj == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (FileObj.Length > 1048576 * 50) //100 MB
                {
                    throw new UserFriendlyException(L("Product_SizeLimit_Error"));
                }

                if (FileObj.ContentType != "text/csv" && FileObj.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && FileObj.ContentType != "application/vnd.ms-excel" && !(FileObj.ContentType == "application/octet-stream" && FileObj.FileName.EndsWith(".xlsx"))) //100 MB
                {
                    throw new UserFriendlyException("Unsupported file type. Please download sample file and try again.");
                }

                byte[] fileBytes;
                using (var stream = FileObj.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    await _BinaryObjectManager.SaveAsync(fileObject);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }

                await _repoProductBulkUploadService.ProductBrandingPositionImport(fileObject.Id);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(L("SomethingWentWrongWhileUpload"));
            }

        }

        [HttpPost]
        [Route("UploadVariantsStockLocationData")]
        public async Task UploadVariantsStockLocationData(IFormFile FileObj)
        {

            // var MetadataFile = Request.Form.Files.First();
            try
            {
                if (FileObj == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (FileObj.Length > 1048576 * 50) //100 MB
                {
                    throw new UserFriendlyException(L("Product_SizeLimit_Error"));
                }

                if (FileObj.ContentType != "text/csv" && FileObj.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && FileObj.ContentType != "application/vnd.ms-excel" && !(FileObj.ContentType == "application/octet-stream" && FileObj.FileName.EndsWith(".xlsx"))) //100 MB
                {
                    throw new UserFriendlyException("Unsupported file type. Please download sample file and try again.");
                }

                byte[] fileBytes;
                using (var stream = FileObj.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    await _BinaryObjectManager.SaveAsync(fileObject);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }

                await _repoProductBulkUploadService.ProductVariantStockLocationImport(fileObject.Id);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(L("SomethingWentWrongWhileUpload"));
            }
        }

    }
}
