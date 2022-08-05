using System;
using System.Collections.Generic;
using System.Drawing;
using Abp.AspNetZeroCore.Net;
using Abp.Collections.Extensions;
using Abp.Dependency;
using CaznerMarketplaceBackendApp.Dto;
using CaznerMarketplaceBackendApp.Storage;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Mozilla;
using static CaznerMarketplaceBackendApp.AppConsts;

namespace CaznerMarketplaceBackendApp.DataExporting.Excel.EpPlus
{
    public abstract class EpPlusExcelExporterBase : CaznerMarketplaceBackendAppAppServiceBase, ITransientDependency
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        protected EpPlusExcelExporterBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        protected FileDto CreateExcelPackage(string fileName, Action<ExcelPackage> creator)
        {
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (var excelPackage = new ExcelPackage())
            {
                creator(excelPackage);
                Save(excelPackage, file);
            }

            return file;
        }

        protected void AddHeader(ExcelWorksheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i + 1, headerTexts[i]);
            }
        }

        protected void AddHeader(ExcelWorksheet sheet, int columnIndex, string headerText)
        {
            sheet.Cells[1, columnIndex].Value = headerText;
            sheet.Cells[1, columnIndex].Style.Font.Bold = true;
        }

        protected void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, EDataType eDataType)
        {
            if (items.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 1; i <= items.Count; i++)
            {
                if(eDataType==EDataType.MainHeading)
                {
                    sheet.Cells[startRowIndex, i].Style.Font.Size = 16;
                    sheet.Cells[startRowIndex, i].Style.Font.Name = "Calibri";
                    sheet.Cells[startRowIndex, i].Style.Font.Bold = true;
                    //sheet.Cells[startRowIndex, i].Style.Font.Color.SetColor(Color.Blue);
                }
                else if (eDataType == EDataType.SubHeading)
                {
                    sheet.Cells[startRowIndex, i].Style.Font.Size = 15;
                    sheet.Cells[startRowIndex, i].Style.Font.Name = "Calibri";
                    sheet.Cells[startRowIndex, i].Style.Font.Bold = true;
                }
                else if (eDataType == EDataType.TableHeader)
                {
                    sheet.Cells[startRowIndex, i].Style.Font.Size = 14;
                    sheet.Cells[startRowIndex, i].Style.Font.Name = "Calibri";
                    sheet.Cells[startRowIndex, i].Style.Font.Bold = true;
                }
                sheet.Cells[startRowIndex, i].Value = items[i-1];
            }
        }

        protected void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
                }
            }
        }

        protected void Save(ExcelPackage excelPackage, FileDto file)
        {
            _tempFileCacheManager.SetFile(file.FileToken, excelPackage.GetAsByteArray());
        }


    }
}