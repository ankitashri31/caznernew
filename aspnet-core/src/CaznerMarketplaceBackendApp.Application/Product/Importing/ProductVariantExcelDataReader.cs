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

namespace FimApp.Authorization.Users.Importing
{
    public class ProductVariantExcelDataReader : EpPlusExcelImporterBase<ImportColorVariantsDto>, IProductVariantExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;
        private readonly IFeatureChecker _featureChecker;

        public ProductVariantExcelDataReader(ILocalizationManager localizationManager, IFeatureChecker featureChecker)
        {
            _localizationSource = localizationManager.GetSource(CaznerMarketplaceBackendAppConsts.LocalizationSourceName);
            _featureChecker = featureChecker;
        }

   
        public List<ImportColorVariantsDto> GetProductsVariantFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessVariantExcelRow);
        }

        private ImportColorVariantsDto ProcessVariantExcelRow(ExcelWorksheet worksheet, int row)
        {

            var exceptionMessage = new StringBuilder();
            var product = new ImportColorVariantsDto();
            List<ColorVariantsModel> listObj = new List<ColorVariantsModel>();
            if (worksheet.Name.ToLower() != "product variant")
            {
                return product;
            }
            try
            {
                string[] NumberOfColumns = { "B", "C", "D" };
                string[] NumberOfAllColumns = { "A", "B", "C", "D", "E", "F", "G", "H" };

                product.ParentProductId = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(product.ParentProductId), exceptionMessage);
                if(!string.IsNullOrEmpty(product.ParentProductId) && product.ParentProductId != null)
                {
                    foreach (var item in NumberOfColumns)
                    {
                        ColorVariantsModel model = new ColorVariantsModel();
                        try
                        {
                            for (int i = 1; i <= 17; i++)
                            {
                                model.ParentProductSKU = product.ParentProductId;
                                switch (i)
                                {
                                    case 1:
                                        int SKUCount = GetColumnByName(worksheet, item + i);
                                        model.Color = GetRequiredValueFromRowOrNull(worksheet, row, SKUCount, item + i, exceptionMessage);

                                        break;

                                    case 2:
                                        int SizeCount = GetColumnByName(worksheet, item + i);
                                        model.Size = GetRequiredValueFromRowOrNull(worksheet, row, SizeCount, item + i, exceptionMessage);

                                        break;

                                    case 3:
                                        int MaterialCount = GetColumnByName(worksheet, item + i);
                                        model.Material = GetRequiredValueFromRowOrNull(worksheet, row, MaterialCount, item + i, exceptionMessage);

                                        break;

                                    case 4:
                                        int StyleCount = GetColumnByName(worksheet, item + i);
                                        model.Style = GetRequiredValueFromRowOrNull(worksheet, row, StyleCount, item + i, exceptionMessage);
                                        break;

                                    case 5:
                                        int VariantSKUCount = GetColumnByName(worksheet, item + i);
                                        model.SKU = GetRequiredValueFromRowOrNull(worksheet, row, VariantSKUCount, item + i, exceptionMessage);
                                        break;

                                    case 6:
                                        int BarcodeCount = GetColumnByName(worksheet, item + i);
                                        model.BarCode = GetRequiredValueFromRowOrNull(worksheet, row, BarcodeCount, item + i, exceptionMessage);
                                        break;

                                    case 7:
                                        int QuantityStockUnitCount = GetColumnByName(worksheet, item + i);
                                        model.QuantityStockUnit = GetRequiredValueFromRowOrNull(worksheet, row, QuantityStockUnitCount, item + i, exceptionMessage);
                                        break;

                                    case 8:
                                        int PriceCount = GetColumnByName(worksheet, item + i);
                                        model.Price = GetRequiredValueFromRowOrNull(worksheet, row, PriceCount, item + i, exceptionMessage);
                                        break;

                                    case 9:
                                        int IsTrackQuantityCount = GetColumnByName(worksheet, item + i);
                                        model.IsTrackQuantity = GetRequiredValueFromRowOrNull(worksheet, row, IsTrackQuantityCount, item + i, exceptionMessage);
                                        break;

                                    case 10:
                                        int IsChargeTaxCount = GetColumnByName(worksheet, item + i);
                                        model.IsChargeTaxVariant = GetRequiredValueFromRowOrNull(worksheet, row, IsChargeTaxCount, item + i, exceptionMessage);
                                        break;

                                    case 11:
                                        int ComparePriceCount = GetColumnByName(worksheet, item + i);
                                        model.ComparePrice = GetRequiredValueFromRowOrNull(worksheet, row, ComparePriceCount, item + i, exceptionMessage);
                                        break;

                                    case 12:
                                        int CostItemCount = GetColumnByName(worksheet, item + i);
                                        model.CostPerItem = GetRequiredValueFromRowOrNull(worksheet, row, CostItemCount, item + i, exceptionMessage);
                                        break;

                                    case 13:
                                        int MarginCount = GetColumnByName(worksheet, item + i);
                                        model.Margin = GetRequiredValueFromRowOrNull(worksheet, row, MarginCount, item + i, exceptionMessage);
                                        break;

                                    case 14:
                                        int ProfitCurrencyCount = GetColumnByName(worksheet, item + i);
                                        model.ProfitCurrencySymbol = GetRequiredValueFromRowOrNull(worksheet, row, ProfitCurrencyCount, item + i, exceptionMessage);
                                        break;

                                    case 15:
                                        int ProfitCount = GetColumnByName(worksheet, item + i);
                                        model.Profit = GetRequiredValueFromRowOrNull(worksheet, row, ProfitCount, item + i, exceptionMessage);
                                        break;

                                    case 16:
                                        int ShapeCount = GetColumnByName(worksheet, item + i);
                                        model.Shape = GetRequiredValueFromRowOrNull(worksheet, row, ShapeCount, item + i, exceptionMessage);
                                        break;

                                    case 17:
                                        int ImagesCount = GetColumnByName(worksheet, item + i);
                                        model.Images = GetRequiredValueFromRowOrNull(worksheet, row, ImagesCount, item + i, exceptionMessage);
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        if (!string.IsNullOrEmpty(model.SKU))
                        {
                            listObj.Add(model);
                        }
                        product.VariantsData = listObj;
                    }
                }
            }
            catch (System.Exception exception)
            {
                // user.Exception = exception.Message;
            }

            return product;
        }

        private int GetColumnByName(ExcelWorksheet ws, string columnName)
        {
            if (ws == null) throw new ArgumentNullException(nameof(ws));
            return ws.Cells["1:1"].First(c => c.Value.ToString().Trim() == columnName.Trim()).Start.Column;
        }


        private ColorVariantsModel ProcessAzureExcelRow(ExcelWorksheet worksheet, int row)
        {
            var exceptionMessage = new StringBuilder();
            var user = new ColorVariantsModel();

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

        private string[] GetAssignedRoleNamesFromRow(ExcelWorksheet worksheet, int row, int column)
        {
            //Uncomment below code when fimapp start
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return new string[0];
            }

            return cellValue.ToString().Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();
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
