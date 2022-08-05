
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.Currency;
using CaznerMarketplaceBackendApp.Product.Dto;
using CaznerMarketplaceBackendApp.Product.Masters;
using FimApp.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
using System.IO;
using System.Diagnostics;
using CaznerMarketplaceBackendApp.Product.Importing.Dto;
using CaznerMarketplaceBackendApp.Storage;
using Abp.Dependency;
using Abp.Runtime.Session;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using CaznerMarketplaceBackendApp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Abp.Data;
using Microsoft.Data.SqlClient;
using CaznerMarketplaceBackendApp.Users;
using CaznerMarketplaceBackendApp.Users.Dto;
using System.Text.RegularExpressions;
using Dapper;
using CaznerMarketplaceBackendApp.BulkColorCodes;


namespace CaznerMarketplaceBackendApp.Product.Importing
{

    #region header constructor callings

    public class ImportHexColorsFromExcel : BackgroundJob<ImportBulkColorCodesJobArgs>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        protected readonly IBinaryObjectManager _BinaryObjectManager;
        private IConfiguration _configuration;
        string DBConnection = string.Empty;
        private CaznerMarketplaceBackendAppDbContext _dbContext;
        private readonly IRepository<HexColorCodesMaster, long> _HexColorCodesMasterRepository;
        private IDbConnection _db;
        public ImportHexColorsFromExcel(IUnitOfWorkManager unitOfWorkManager, IBinaryObjectManager BinaryObjectManager, IConfiguration configuration, CaznerMarketplaceBackendAppDbContext dbContext, IRepository<HexColorCodesMaster, long> HexColorCodesMasterRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _BinaryObjectManager = BinaryObjectManager;
            _configuration = configuration;
            DBConnection = _configuration["ConnectionStrings:Default"];
            _dbContext = dbContext;
            _HexColorCodesMasterRepository = HexColorCodesMasterRepository;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);

        }

        #endregion

        [UnitOfWork]
        public override void Execute(ImportBulkColorCodesJobArgs args)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    AsyncHelper.RunSync(() => CreateHexColorsUsingTemp());

                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }

        }
        public virtual async Task CreateHexColorsUsingTemp()
        {
            try
            {

                List<HexColorCodesMaster> MasterColorList = new List<HexColorCodesMaster>();

                var BulkColorsFromExcel = await _db.QueryAsync<HexColorCodesMaster>("Select * from HexColorCodesRawData Where Color IS NOT NULL AND HexCode IS NOT NULL", new
                {

                }, commandType: System.Data.CommandType.Text);
                var MasterBulkColors = await _db.QueryAsync<HexColorCodesMaster>("Select * from HexColorCodesMaster", new
                {

                }, commandType: System.Data.CommandType.Text);

                List<HexColorCodesMaster> distinctExcelCode = BulkColorsFromExcel.Where(i => !string.IsNullOrEmpty(i.Color)).GroupBy(p => p.Color.ToLower().Trim()).Select(g => g.LastOrDefault()).ToList();
                List<HexColorCodesMaster> NewColorsToBeInserted = new List<HexColorCodesMaster>();
                List<HexColorCodesMaster> FinalColorsList = new List<HexColorCodesMaster>();
                List<HexColorCodesMaster> FinalColorsToUpdate = new List<HexColorCodesMaster>();
                if (MasterBulkColors.ToList().Count() > 0)
                {
                    var dataToInsert = distinctExcelCode.Select(v => v.Color.ToLower()).ToList().Except(MasterBulkColors.Select(b => b.Color.ToLower()).ToList()).ToList();
                    var DataToUpdate = distinctExcelCode.Where(x => !dataToInsert.Contains(x.Color)).ToList();
                    NewColorsToBeInserted = distinctExcelCode.Where(x => dataToInsert.Contains(x.Color.ToLower())).ToList();

                    #region update colors

                    foreach (var item in DataToUpdate)
                    {

                        var UpdatedColor = MasterBulkColors.ToList().Where(i => i.Color == item.Color && i.ColorFamily == item.ColorFamily).FirstOrDefault();
                        if (UpdatedColor != null)
                        {
                            UpdatedColor.HexCode = item.HexCode;
                            UpdatedColor.ColorFamily = item.ColorFamily;
                            FinalColorsToUpdate.Add(UpdatedColor);
                        }
                        else
                        {
                            HexColorCodesMaster Code = new HexColorCodesMaster();
                            Code.ColorFamily = item.ColorFamily;
                            Code.Color = item.Color;
                            Code.HexCode = item.HexCode;
                            FinalColorsList.Add(Code);
                        }
                    }
                    if (FinalColorsToUpdate.Count > 0)
                    {
                        await UpdateHexColorMaster(FinalColorsToUpdate);
                    }
                    #endregion

                }
                else
                {
                    NewColorsToBeInserted = distinctExcelCode.ToList();
                }

                #region Insert color codes

                FinalColorsList = (from data in NewColorsToBeInserted
                                   select new HexColorCodesMaster
                                   {
                                       HexCode = data.HexCode,
                                       Color = data.Color,
                                       ColorFamily = data.ColorFamily
                                   }).ToList();


                await CreateHexColorMaster(FinalColorsList);


                #region delete raw temp data

                await _db.QueryAsync<HexColorCodesRawData>("delete from HexColorCodesRawData DBCC CHECKIDENT ('HexColorCodesRawData', RESEED, 1)", new
                {

                }, commandType: System.Data.CommandType.Text);

                #endregion
            }
            catch (Exception ex)
            {

            }

            #endregion

        }

        private async Task CreateHexColorMaster(List<HexColorCodesMaster> HexColorList)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _HexColorCodesMasterRepository.BulkInsertAsync(HexColorList);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task UpdateHexColorMaster(List<HexColorCodesMaster> HexColorList)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _HexColorCodesMasterRepository.BulkUpdate(HexColorList);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
