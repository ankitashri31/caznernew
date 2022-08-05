using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using CaznerMarketplaceBackendApp.Authorization.Users.Importing.Dto;
using FimApp.DataExporting.Excel.EpPlus;
using System;
using System.Globalization;
using Abp.Application.Features;
using CaznerMarketplaceBackendApp;
using CaznerMarketplaceBackendApp.Product.Importing;
using CaznerMarketplaceBackendApp.Product.Dto;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Runtime.Serialization.Formatters.Binary;

namespace FimApp.Authorization.Users.Importing
{
    public class ProductListExcelDataReader : EpPlusExcelImporterBase<ImportBulkProductDto>, IProductListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;
        private readonly IFeatureChecker _featureChecker;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _Environment;

        int i = 0;
        long MSValue = 0;
        public ProductListExcelDataReader(ILocalizationManager localizationManager, IFeatureChecker featureChecker, IWebHostEnvironment Environment)
        {
            _localizationSource = localizationManager.GetSource(CaznerMarketplaceBackendAppConsts.LocalizationSourceName);
            _featureChecker = featureChecker;
            _Environment = Environment;
        }

        public List<ImportBulkProductDto> GetProductsFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        public List<ImportBulkProductDto> GetProductsFromAzureExcel(byte[] fileBytes)
        {
            return ProcessAzueExcelFile(fileBytes, ProcessAzureExcelRow);
        }

        

        private ImportBulkProductDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            var exceptionMessage = new StringBuilder();
            var product = new ImportBulkProductDto();
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var IsSKUExists = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(product.ParentSKU), exceptionMessage);
            if(!string.IsNullOrEmpty(IsSKUExists))
            {
                try
                {
                    product.MainorVariant = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(product.MainorVariant), exceptionMessage);
                    if (product.MainorVariant.ToLower() == "main")
                    {
                        product.ParentSKU = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(product.ParentSKU), exceptionMessage);
                        product.VariantSKU = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(product.VariantSKU), exceptionMessage);
                        product.Name = GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(product.Name), exceptionMessage);
                        product.Groupcategory = GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(product.Groupcategory), exceptionMessage);
                        product.Productcategory = GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(product.Productcategory), exceptionMessage);
                        product.ProductCollections = GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(product.ProductCollections), exceptionMessage);
                        product.ShortDescription = GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(product.ShortDescription), exceptionMessage);
                        product.Description = GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(product.Description), exceptionMessage);

                        product.Features = GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(product.Features), exceptionMessage);
                        product.Caznerproducttypes = GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(product.Caznerproducttypes), exceptionMessage);
                        product.ProductSize = GetRequiredValueFromRowOrNull(worksheet, row, 13, nameof(product.ProductSize), exceptionMessage);
                        product.Colour = GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(product.Colour), exceptionMessage);
                        product.ProductMaterial = GetRequiredValueFromRowOrNull(worksheet, row, 15, nameof(product.ProductMaterial), exceptionMessage);
                        product.Style = GetRequiredValueFromRowOrNull(worksheet, row, 16, nameof(product.Style), exceptionMessage);
                        product.ProductBrands = GetRequiredValueFromRowOrNull(worksheet, row, 17, nameof(product.ProductBrands), exceptionMessage);
                        product.ProductTags = GetRequiredValueFromRowOrNull(worksheet, row, 18, nameof(product.ProductTags), exceptionMessage);
                        product.ProductVendors = GetRequiredValueFromRowOrNull(worksheet, row, 19, nameof(product.ProductVendors), exceptionMessage);
                        product.UnitPrice = GetRequiredValueFromRowOrNull(worksheet, row, 20, nameof(product.UnitPrice), exceptionMessage);

                        product.CostPerItem = GetRequiredValueFromRowOrNull(worksheet, row, 21, nameof(product.CostPerItem), exceptionMessage);
                        product.IsOnSale = GetRequiredValueFromRowOrNull(worksheet, row, 22, nameof(product.IsOnSale), exceptionMessage);
                        product.DiscountPercentage = GetRequiredValueFromRowOrNull(worksheet, row, 23, nameof(product.DiscountPercentage), exceptionMessage);
                        product.SalePrice = GetRequiredValueFromRowOrNull(worksheet, row, 24, nameof(product.SalePrice), exceptionMessage);

                        product.Freigthnote = GetRequiredValueFromRowOrNull(worksheet, row, 25, nameof(product.Freigthnote), exceptionMessage);
                        product.DepositRequired = GetRequiredValueFromRowOrNull(worksheet, row, 26, nameof(product.DepositRequired), exceptionMessage);
                        product.IsChargeTax = GetRequiredValueFromRowOrNull(worksheet, row, 27, nameof(product.IsChargeTax), exceptionMessage);
                        product.IsProductHasPriceVariant = GetRequiredValueFromRowOrNull(worksheet, row, 28, nameof(product.IsProductHasPriceVariant), exceptionMessage);
                        product.Barcode = GetRequiredValueFromRowOrNull(worksheet, row, 29, nameof(product.Barcode), exceptionMessage);
                        product.TotalNoAvailable = GetRequiredValueFromRowOrNull(worksheet, row, 30, nameof(product.TotalNoAvailable), exceptionMessage);

                        product.AlertRestockAtThisNumber = GetRequiredValueFromRowOrNull(worksheet, row, 31, nameof(product.AlertRestockAtThisNumber), exceptionMessage);
                        product.IsTrackQuantity = GetRequiredValueFromRowOrNull(worksheet, row, 32, nameof(product.IsTrackQuantity), exceptionMessage);
                        product.IsStopSellingStockZero = GetRequiredValueFromRowOrNull(worksheet, row, 33, nameof(product.IsStopSellingStockZero), exceptionMessage);
                        product.ProductHeight = GetRequiredValueFromRowOrNull(worksheet, row, 34, nameof(product.ProductHeight), exceptionMessage);
                        product.ProductWidth = GetRequiredValueFromRowOrNull(worksheet, row, 35, nameof(product.ProductWidth), exceptionMessage);
                        product.Productlength = GetRequiredValueFromRowOrNull(worksheet, row, 36, nameof(product.Productlength), exceptionMessage);
                        product.ProductDiameter = GetRequiredValueFromRowOrNull(worksheet, row, 37, nameof(product.ProductDiameter), exceptionMessage);
                        product.ProductUOM = GetRequiredValueFromRowOrNull(worksheet, row, 38, nameof(product.ProductUOM), exceptionMessage);
                        product.ProductUnitWeight = GetRequiredValueFromRowOrNull(worksheet, row, 39, nameof(product.ProductUnitWeight), exceptionMessage);
                        product.WeightUOM = GetRequiredValueFromRowOrNull(worksheet, row, 40, nameof(product.WeightUOM), exceptionMessage);

                        product.VolumeValue = GetRequiredValueFromRowOrNull(worksheet, row, 41, nameof(product.VolumeValue), exceptionMessage);
                        product.VolumeUOM = GetRequiredValueFromRowOrNull(worksheet, row, 42, nameof(product.VolumeUOM), exceptionMessage);
                        product.ProductDimensionNotes = GetRequiredValueFromRowOrNull(worksheet, row, 43, nameof(product.ProductDimensionNotes), exceptionMessage);
                        product.IfsetOtherproducttitleanddimensoionsinthisset = GetRequiredValueFromRowOrNull(worksheet, row, 44, nameof(product.IfsetOtherproducttitleanddimensoionsinthisset), exceptionMessage);
                        product.ProductPackaging = GetRequiredValueFromRowOrNull(worksheet, row, 45, nameof(product.ProductPackaging), exceptionMessage);
                        product.Counrtyoforigin = GetRequiredValueFromRowOrNull(worksheet, row, 46, nameof(product.Counrtyoforigin), exceptionMessage);
                        product.IsPhysicalProduct = GetRequiredValueFromRowOrNull(worksheet, row, 47, nameof(product.IsPhysicalProduct), exceptionMessage);
                        product.CartonQuantity = GetRequiredValueFromRowOrNull(worksheet, row, 48, nameof(product.CartonQuantity), exceptionMessage);
                        product.CartonLength = GetRequiredValueFromRowOrNull(worksheet, row, 49, nameof(product.CartonLength), exceptionMessage);
                        product.CartonWidth = GetRequiredValueFromRowOrNull(worksheet, row, 50, nameof(product.CartonWidth), exceptionMessage);

                        product.CartonHeight = GetRequiredValueFromRowOrNull(worksheet, row, 51, nameof(product.CartonHeight), exceptionMessage);
                        product.CartonUOM = GetRequiredValueFromRowOrNull(worksheet, row, 52, nameof(product.CartonUOM), exceptionMessage);
                        product.CartonWeight = GetRequiredValueFromRowOrNull(worksheet, row, 53, nameof(product.CartonWeight), exceptionMessage);
                        product.CartonWeightUOM = GetRequiredValueFromRowOrNull(worksheet, row, 54, nameof(product.CartonWeightUOM), exceptionMessage);
                        product.CartonPackaging = GetRequiredValueFromRowOrNull(worksheet, row, 55, nameof(product.CartonPackaging), exceptionMessage);
                        product.CartonNote = GetRequiredValueFromRowOrNull(worksheet, row, 56, nameof(product.CartonNote), exceptionMessage);
                        product.CartonCubicWeight = GetRequiredValueFromRowOrNull(worksheet, row, 57, nameof(product.CartonCubicWeight), exceptionMessage);
                        product.PalletWeight = GetRequiredValueFromRowOrNull(worksheet, row, 58, nameof(product.PalletWeight), exceptionMessage);
                        product.CartonsPerPallet = GetRequiredValueFromRowOrNull(worksheet, row, 59, nameof(product.CartonsPerPallet), exceptionMessage);
                        // product.UnitsPerProduct = GetRequiredValueFromRowOrNull(worksheet, row, 60, nameof(product.UnitsPerProduct), exceptionMessage);
                        product.UnitsPerPallet = GetRequiredValueFromRowOrNull(worksheet, row, 60, nameof(product.UnitsPerPallet), exceptionMessage);

                        product.PalletNote = GetRequiredValueFromRowOrNull(worksheet, row, 61, nameof(product.PalletNote), exceptionMessage);
                        product.MainProductImage = GetRequiredValueFromRowOrNull(worksheet, row, 62, nameof(product.MainProductImage), exceptionMessage);
                        product.ProductImages = GetRequiredValueFromRowOrNull(worksheet, row, 63, nameof(product.ProductImages), exceptionMessage);
                        product.LineMediaArtImages = GetRequiredValueFromRowOrNull(worksheet, row, 64, nameof(product.LineMediaArtImages), exceptionMessage);
                        product.LifeStyleImages = GetRequiredValueFromRowOrNull(worksheet, row, 65, nameof(product.LifeStyleImages), exceptionMessage);
                        product.OtherMediaImages = GetRequiredValueFromRowOrNull(worksheet, row, 66, nameof(product.OtherMediaImages), exceptionMessage);
                        product.NumberofPieces = GetRequiredValueFromRowOrNull(worksheet, row, 67, nameof(product.NumberofPieces), exceptionMessage);
                        product.MOQ = GetRequiredValueFromRowOrNull(worksheet, row, 68, nameof(product.MOQ), exceptionMessage);

                        //product.UnitOfMeasure = GetRequiredValueFromRowOrNull(worksheet, row, 67, nameof(product.UnitOfMeasure), exceptionMessage);
                        // in between all the columns are for price and quantities
                        product.status = GetRequiredValueFromRowOrNull(worksheet, row, 85, nameof(product.status), exceptionMessage);
                        product.IsIndentOrder = GetRequiredValueFromRowOrNull(worksheet, row, 86, nameof(product.IsIndentOrder), exceptionMessage);
                        product.Published = GetRequiredValueFromRowOrNull(worksheet, row, 87, nameof(product.Published), exceptionMessage);
                        product.TurnAroundTime = GetRequiredValueFromRowOrNull(worksheet, row, 88, nameof(product.TurnAroundTime), exceptionMessage);
                        product.RelatedProducts = GetRequiredValueFromRowOrNull(worksheet, row, 89, nameof(product.RelatedProducts), exceptionMessage);
                        product.AlternativeProducts = GetRequiredValueFromRowOrNull(worksheet, row, 90, nameof(product.AlternativeProducts), exceptionMessage);

                        product.ColourFamily = GetRequiredValueFromRowOrNull(worksheet, row, 91, nameof(product.ColourFamily), exceptionMessage);
                        product.PMSColourCode = GetRequiredValueFromRowOrNull(worksheet, row, 92, nameof(product.PMSColourCode), exceptionMessage);
                        product.VideoURL = GetRequiredValueFromRowOrNull(worksheet, row, 93, nameof(product.VideoURL), exceptionMessage);
                        product.Image360Degrees = GetRequiredValueFromRowOrNull(worksheet, row, 94, nameof(product.Image360Degrees), exceptionMessage);
                        product.ProductViews = GetRequiredValueFromRowOrNull(worksheet, row, 95, nameof(product.ProductViews), exceptionMessage);
                        product.NextShipmentDate = GetRequiredValueFromRowOrNull(worksheet, row, 96, nameof(product.NextShipmentDate), exceptionMessage);
                        product.NextShipmentQuantity = GetRequiredValueFromRowOrNull(worksheet, row, 97, nameof(product.NextShipmentQuantity), exceptionMessage);
                        product.ExtraSetupFee = GetRequiredValueFromRowOrNull(worksheet, row, 98, nameof(product.ExtraSetupFee), exceptionMessage);
                        product.BrandingMethodsseparatedbyacommarelevanttothisproduct = GetRequiredValueFromRowOrNull(worksheet, row, 99, nameof(product.BrandingMethodsseparatedbyacommarelevanttothisproduct), exceptionMessage);
                        product.BrandingMethodNote = GetRequiredValueFromRowOrNull(worksheet, row, 100, nameof(product.BrandingMethodNote), exceptionMessage);
                        product.BrandingUOM = GetRequiredValueFromRowOrNull(worksheet, row, 101, nameof(product.BrandingUOM), exceptionMessage);

                        List<QuantityPriceVariantModel> PriceQuantityData = new List<QuantityPriceVariantModel>();
                        for (int i = 1; i <= 8; i++)
                        {
                            QuantityPriceVariantModel model = new QuantityPriceVariantModel();
                            int QuantityRowCount = GetColumnByName(worksheet, "Q" + i);
                            string Qty = GetRequiredValueFromRowOrNull(worksheet, row, QuantityRowCount, "Q" + i, exceptionMessage);

                            if (string.IsNullOrEmpty(Qty))
                            {
                                break;
                            }
                            else
                            {
                                model.Quantity = Qty;
                                int PriceRowCount = GetColumnByName(worksheet, "P" + i);
                                model.Price = GetRequiredValueFromRowOrNull(worksheet, row, PriceRowCount, "P" + i, exceptionMessage);

                                if (model != null)
                                {
                                    if (!string.IsNullOrEmpty(model.Quantity) && !string.IsNullOrEmpty(model.Price))
                                    {
                                        PriceQuantityData.Add(model);
                                    }
                                }
                            }
                        }

                        product.QuantityPriceVariantModel = PriceQuantityData;
                        List<BrandingLocationModel> BrandingLocations = new List<BrandingLocationModel>();

                        for (int i = 1; i <= 20; i++)
                        {
                            BrandingLocationModel model = new BrandingLocationModel();

                            int LocationTitle = GetColumnByName(worksheet, "Branding_Location_Title_" + i);
                            string Title = GetRequiredValueFromRowOrNull(worksheet, row, LocationTitle, "Branding_Location_Title_" + i, exceptionMessage);
                            if (string.IsNullOrEmpty(Title))
                            {
                                break;
                            }
                            else
                            {
                                model.Branding_Location_Title_ = Title;

                                int PositionMaxWidth = GetColumnByName(worksheet, "Position_Max_Width_" + i);
                                int PositionMaxHight = GetColumnByName(worksheet, "Position_Max_Height_" + i);
                                int LocationImage = GetColumnByName(worksheet, "Branding_Location_Image_" + i);
                                model.Position_Max_Height_ = GetRequiredValueFromRowOrNull(worksheet, row, PositionMaxHight, "Position_Max_Height_" + i, exceptionMessage);
                                model.Position_Max_Width_ = GetRequiredValueFromRowOrNull(worksheet, row, PositionMaxWidth, "Position_Max_Width_" + i, exceptionMessage);
                                model.Branding_Location_Image_ = GetRequiredValueFromRowOrNull(worksheet, row, LocationImage, "Branding_Location_Image_" + i, exceptionMessage);
                                BrandingLocations.Add(model);
                            }
                        }
                        product.BrandingLocationModel = BrandingLocations;
                    }
                    else
                    {
                        product.ParentSKU = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(product.ParentSKU), exceptionMessage);
                        product.VariantSKU = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(product.VariantSKU), exceptionMessage);
                        product.ProductSize = GetRequiredValueFromRowOrNull(worksheet, row, 13, nameof(product.ProductSize), exceptionMessage);
                        product.Colour = GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(product.Colour), exceptionMessage);
                        product.ProductMaterial = GetRequiredValueFromRowOrNull(worksheet, row, 15, nameof(product.ProductMaterial), exceptionMessage);
                        product.Style = GetRequiredValueFromRowOrNull(worksheet, row, 16, nameof(product.Style), exceptionMessage);
                        product.UnitPrice = GetRequiredValueFromRowOrNull(worksheet, row, 20, nameof(product.UnitPrice), exceptionMessage);
                        product.CostPerItem = GetRequiredValueFromRowOrNull(worksheet, row, 21, nameof(product.CostPerItem), exceptionMessage);
                        product.IsOnSale = GetRequiredValueFromRowOrNull(worksheet, row, 22, nameof(product.IsOnSale), exceptionMessage);
                        product.DiscountPercentage = GetRequiredValueFromRowOrNull(worksheet, row, 23, nameof(product.DiscountPercentage), exceptionMessage);
                        product.SalePrice = GetRequiredValueFromRowOrNull(worksheet, row, 24, nameof(product.SalePrice), exceptionMessage);
                        product.IsChargeTax = GetRequiredValueFromRowOrNull(worksheet, row, 27, nameof(product.IsChargeTax), exceptionMessage);
                        product.Barcode = GetRequiredValueFromRowOrNull(worksheet, row, 29, nameof(product.Barcode), exceptionMessage);
                        product.IsTrackQuantity = GetRequiredValueFromRowOrNull(worksheet, row, 32, nameof(product.IsTrackQuantity), exceptionMessage);
                        product.MainProductImage = GetRequiredValueFromRowOrNull(worksheet, row, 62, nameof(product.MainProductImage), exceptionMessage);
                        product.NextShipmentDate = GetRequiredValueFromRowOrNull(worksheet, row, 96, nameof(product.NextShipmentDate), exceptionMessage);
                        product.NextShipmentQuantity = GetRequiredValueFromRowOrNull(worksheet, row, 97, nameof(product.NextShipmentQuantity), exceptionMessage);

                        List<QuantityPriceVariantModel> PriceQuantityData = new List<QuantityPriceVariantModel>();
                        for (int i = 1; i <= 8; i++)
                        {
                            QuantityPriceVariantModel model = new QuantityPriceVariantModel();
                            int QuantityRowCount = GetColumnByName(worksheet, "Q" + i);
                            string Qty = GetRequiredValueFromRowOrNull(worksheet, row, QuantityRowCount, "Q" + i, exceptionMessage);

                            if (string.IsNullOrEmpty(Qty))
                            {
                                break;
                            }
                            else
                            {
                                model.Quantity = Qty;
                                int PriceRowCount = GetColumnByName(worksheet, "P" + i);
                                model.Price = GetRequiredValueFromRowOrNull(worksheet, row, PriceRowCount, "P" + i, exceptionMessage);

                                if (model != null)
                                {
                                    if (!string.IsNullOrEmpty(model.Quantity) && !string.IsNullOrEmpty(model.Price))
                                    {
                                        PriceQuantityData.Add(model);
                                    }
                                }
                            }
                        }

                        product.QuantityPriceVariantModel = PriceQuantityData;

                    }
                    watch.Stop();
                    MSValue = MSValue + + watch.ElapsedMilliseconds;
                    Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
                   
                    CreateErrorLogsWithException("Line no 194", "Exec time total: "+ MSValue +"  Per reading request time taking : "+ watch.ElapsedMilliseconds +" iteration number : "+ i++ , "");

                }
                catch(Exception ex)
                {

                    string exception = ex.StackTrace + "________________________________" + ex.Message;
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                    CreateErrorLogsWithException(line.ToString(), ex.Message, ex.StackTrace);
                }
            }
            return product;
        }

        public void CreateErrorLogsWithException(string Path, string message, string Ex)
        {
            try
            {
                var fileSavePath = _Environment.WebRootPath + "//swagger//ErrorLogs.txt";
                //Creating the path where pdf format of forms will be save.
                DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);



                // check if file exists and date is same then write error log in same file
                if (File.Exists(fileSavePath))
                {



                    //write to same file
                    using (StreamWriter SW = File.AppendText(fileSavePath))
                    {
                        try
                        {
                            SW.WriteLine(DateTime.Now + " ------------------------------------------------------------------------------------------------------------------");



                            SW.WriteLine(message);
                            if (!string.IsNullOrEmpty(Path))
                            {
                                SW.WriteLine("Tesseract path: " + Path);
                            }
                            if (!string.IsNullOrEmpty(Ex))
                            {
                                SW.WriteLine("Error: " + Ex);
                            }



                            //SW.Close();
                        }
                        catch (Exception exc)
                        {
                          
                        }
                        finally
                        {
                            if (SW != null)
                            {
                                SW.Flush();
                                SW.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                //CreateErrorLogs("line no 415", exc.Message);
            }
        }

        public int GetColumnByName(ExcelWorksheet ws, string columnName)
        {
            if (ws == null) throw new ArgumentNullException(nameof(ws));
            try
            {
                return ws.Cells["1:1"].First(c => c.Value.ToString().Trim() == columnName.Trim()).Start.Column;
            }
            catch(Exception ex)
            {

            }
            return ws.Cells["1:1"].First(c => c.Value.ToString().Trim() == columnName.Trim()).Start.Column;
        }


        private ImportBulkProductDto ProcessAzureExcelRow(ExcelWorksheet worksheet, int row)
        {

            var exceptionMessage = new StringBuilder();
            var user = new ImportBulkProductDto();

            try
            {
                //user.UserName = GetRequiredValueFromRowOrNull(worksheet, row, 2, "givenName", exceptionMessage);
                //user.Name = GetRequiredValueFromRowOrNull(worksheet, row, 2, "givenName", exceptionMessage);
                //user.Surname = GetRequiredValueFromRowOrNull(worksheet, row, 3, "sn", exceptionMessage);
                //user.EmailAddress = GetRequiredValueFromRowOrNull(worksheet, row, 4, "mail", exceptionMessage);
                //user.EmployeeId = GetRequiredValueFromRowOrNull(worksheet, row, 7, "employeeNumber", exceptionMessage);
                //user.SendActivationLink = true;
                //user.ShouldChangePasswordOnNextLogin = true;                
            }
            catch (System.Exception exception)
            {
               // user.Exception = exception.Message;
            }

            return user;
        }

        private string GetRequiredValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return cellValue.ToString();
            }

            exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }

        private DateTime? GetRequiredDateValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                if (cellValue.ToString().Length >= 10
                    && cellValue.ToString().Contains("/")
                    && cellValue.ToString().Split("/").Length == 3)
                {
                    var datePart = cellValue.ToString().Split("/");
                    return new DateTime(Convert.ToInt32(datePart[2].Substring(0, 4)), Convert.ToInt32(datePart[0]), Convert.ToInt32(datePart[1]));
                }
                else
                {
                    //your condition fail code goes here
                    exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
                    return null;
                }
            }

            exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }

    
        private string GetLocalizedExceptionMessagePart(string parameter)
        {
            return _localizationSource.GetString("{0}IsInvalid", _localizationSource.GetString(parameter)) + "; ";
        }

        private bool IsRowEmpty(ExcelWorksheet worksheet, int row)
        {
            return worksheet.Cells[row, 1].Value == null || string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value.ToString());
        }
    }
}
