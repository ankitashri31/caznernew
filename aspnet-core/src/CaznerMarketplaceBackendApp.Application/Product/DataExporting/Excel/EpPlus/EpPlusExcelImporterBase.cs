using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;

namespace FimApp.DataExporting.Excel.EpPlus
{
    public abstract class EpPlusExcelImporterBase<TEntity>
    {
        //private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _Environment;

        public List<TEntity> ProcessExcelFile(byte[] fileBytes, Func<ExcelWorksheet, int, TEntity> processExcelRow)
        {
            var entities = new List<TEntity>();
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var stream = new MemoryStream(fileBytes))
            {                                                            
                using (var excelPackage = new ExcelPackage(stream))
                {
                    int i = 0;
                    foreach (var worksheet in excelPackage.Workbook.Worksheets)
                    {
                        //CreateErrorLogsWithException("Line no 23", "ProcessExcelFile executed - " + i++, "");
                        var entitiesInWorksheet = ProcessWorksheet(worksheet, processExcelRow);

                        entities.AddRange(entitiesInWorksheet);
                    }
                }
            }

            return entities;
        }
        int i = 0;
        private List<TEntity> ProcessWorksheet(ExcelWorksheet worksheet, Func<ExcelWorksheet, int, TEntity> processExcelRow)
        {
            var entities = new List<TEntity>();
            //CreateErrorLogsWithException("Line no 36","ProcessWorkSheet Executed - "+ i++, "");

            for (var i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                try
                {
                    var entity = processExcelRow(worksheet, i);

                    if (entity != null)
                    {
                        entities.Add(entity);
                    }
                }
                catch (Exception ex)
                {
                    //ignore
                    string exception = ex.StackTrace + "________________________________" + ex.Message;
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                   // CreateErrorLogsWithException(line.ToString(), ex.Message, ex.StackTrace);
                }
            }
            //CreateErrorLogsWithException("Line no 54", "ProcessWorkSheet ended - " + i++, "");

            return entities;
        }



        //public void CreateErrorLogsWithException(string Path, string message, string Ex)
        //{
        //    try
        //    {
        //        var fileSavePath = _Environment.WebRootPath + "//swagger//ErrorLogs.txt";
        //        //Creating the path where pdf format of forms will be save.
        //        DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);



        //        // check if file exists and date is same then write error log in same file
        //        if (File.Exists(fileSavePath))
        //        {



        //            //write to same file
        //            using (StreamWriter SW = File.AppendText(fileSavePath))
        //            {
        //                try
        //                {
        //                    SW.WriteLine(DateTime.Now + " ------------------------------------------------------------------------------------------------------------------");



        //                    SW.WriteLine(message);
        //                    if (!string.IsNullOrEmpty(Path))
        //                    {
        //                        SW.WriteLine("Tesseract path: " + Path);
        //                    }
        //                    if (!string.IsNullOrEmpty(Ex))
        //                    {
        //                        SW.WriteLine("Error: " + Ex);
        //                    }



        //                    //SW.Close();
        //                }
        //                catch (Exception exc)
        //                {

        //                }
        //                finally
        //                {
        //                    if (SW != null)
        //                    {
        //                        SW.Flush();
        //                        SW.Close();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        //CreateErrorLogs("line no 415", exc.Message);
        //    }
        //}

        public List<TEntity> ProcessAzueExcelFile(byte[] fileBytes, Func<ExcelWorksheet, int, TEntity> processAzureExcelRow)
        {
            var entities = new List<TEntity>();

            using (var stream = new MemoryStream(fileBytes))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    foreach (var worksheet in excelPackage.Workbook.Worksheets)
                    {
                        var entitiesInWorksheet = ProcessAzureWorksheet(worksheet, processAzureExcelRow);

                        entities.AddRange(entitiesInWorksheet);
                    }
                }
            }

            return entities;
        }

        private List<TEntity> ProcessAzureWorksheet(ExcelWorksheet worksheet, Func<ExcelWorksheet, int, TEntity> processAzureExcelRow)
        {
            var entities = new List<TEntity>();

            for (var i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                try
                {
                    var entity = processAzureExcelRow(worksheet, i);

                    if (entity != null)
                    {
                        entities.Add(entity);
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            }

            return entities;
        }
    }
}
