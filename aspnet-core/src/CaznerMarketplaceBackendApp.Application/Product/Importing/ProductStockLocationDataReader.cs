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
    public class ProductStockLocationDataReader : EpPlusExcelImporterBase<ImportBulkProductStockLocationDto>, IProductStockLocationDataReader
    {
        private readonly ILocalizationSource _localizationSource;
        private readonly IFeatureChecker _featureChecker;

        public ProductStockLocationDataReader(ILocalizationManager localizationManager, IFeatureChecker featureChecker)
        {
            _localizationSource = localizationManager.GetSource(CaznerMarketplaceBackendAppConsts.LocalizationSourceName);
            _featureChecker = featureChecker;
        }

        public List<ImportBulkProductStockLocationDto> GetStockLocationDataFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        public List<ImportBulkProductStockLocationDto> GetProductsFromAzureExcel(byte[] fileBytes)
        {
            return ProcessAzueExcelFile(fileBytes, ProcessAzureExcelRow);
        }
        private ImportBulkProductStockLocationDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {

            var exceptionMessage = new StringBuilder();
            var StockLocation = new ImportBulkProductStockLocationDto();
            List<WareHouseLocationDataDto> StockLocationModel = new List<WareHouseLocationDataDto>();
            var IsSKUExists = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(StockLocation.ProductParentSKU), exceptionMessage);
            if(!string.IsNullOrEmpty(IsSKUExists))
            {
                string[] NumberOfColumns = { "B", "C", "D","E","F" };

                StockLocation.ProductParentSKU = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(StockLocation.ProductParentSKU), exceptionMessage);
                if (!string.IsNullOrEmpty(StockLocation.ProductParentSKU) && StockLocation.ProductParentSKU != null)
                {
                    foreach (var item in NumberOfColumns)
                    {
                        WareHouseLocationDataDto model = new WareHouseLocationDataDto();
                        try
                        {
                            model.ProductParentSKU = StockLocation.ProductParentSKU;

                            for (int i = 1; i <= 6; i++)
                            {
                                switch (i)
                                {
                                    case 1:
                                        int SKUCount = GetColumnByName(worksheet, item + i);
                                        model.StockKeepingUnit = GetRequiredValueFromRowOrNull(worksheet, row, SKUCount, item + i, exceptionMessage);

                                        break;

                                    case 2:
                                        int QtyThisLocCount = GetColumnByName(worksheet, item + i);
                                        model.QuantityAtLocation = GetRequiredValueFromRowOrNull(worksheet, row, QtyThisLocCount, item + i, exceptionMessage);

                                        break;

                                    case 3:
                                        int StockAlertCount = GetColumnByName(worksheet, item + i);
                                        model.StockAlertQty = GetRequiredValueFromRowOrNull(worksheet, row, StockAlertCount, item + i, exceptionMessage);

                                        break;

                                    case 4:
                                        int LocationACount = GetColumnByName(worksheet, item + i);
                                        model.LocationA = GetRequiredValueFromRowOrNull(worksheet, row, LocationACount, item + i, exceptionMessage);
                                        break;

                                    case 5:
                                        int LocationBCount = GetColumnByName(worksheet, item + i);
                                        model.LocationB = GetRequiredValueFromRowOrNull(worksheet, row, LocationBCount, item + i, exceptionMessage);
                                        break;

                                    case 6:
                                        int LocationCCount = GetColumnByName(worksheet, item + i);
                                        model.LocationC = GetRequiredValueFromRowOrNull(worksheet, row, LocationCCount, item + i, exceptionMessage);
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        if (!string.IsNullOrEmpty(model.ProductParentSKU))
                        {
                            StockLocationModel.Add(model);
                        }
                        StockLocation.StockLocationList = StockLocationModel;
                    }
                }
            }
            return StockLocation;
        }

        public int GetColumnByName(ExcelWorksheet ws, string columnName)
        {
            if (ws == null) throw new ArgumentNullException(nameof(ws));
            return ws.Cells["1:1"].First(c => c.Value.ToString().Trim() == columnName.Trim()).Start.Column;
        }


        private ImportBulkProductStockLocationDto ProcessAzureExcelRow(ExcelWorksheet worksheet, int row)
        {
            var exceptionMessage = new StringBuilder();
            var user = new ImportBulkProductStockLocationDto();

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
            //Temp code for cOI only. Comment Below code when fimapp start
            //return new string[] { "COI Staff" };


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
