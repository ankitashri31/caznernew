
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
using CaznerMarketplaceBackendApp.BulkStatesRawImport.Dto;
using CaznerMarketplaceBackendApp.Country;

namespace CaznerMarketplaceBackendApp.Product.Importing
{

    #region header constructor callings

    public class ImportStatesFromExcel : BackgroundJob<ImportStatesJobArgs>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        protected readonly IBinaryObjectManager _BinaryObjectManager;
        private IConfiguration _configuration;
        string DBConnection = string.Empty;
        private CaznerMarketplaceBackendAppDbContext _dbContext;
        private readonly IRepository<States, long> _StatesRepository;
        private IDbConnection _db;
        public ImportStatesFromExcel(IUnitOfWorkManager unitOfWorkManager, IBinaryObjectManager BinaryObjectManager, IConfiguration configuration, CaznerMarketplaceBackendAppDbContext dbContext, IRepository<States, long> StatesRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _BinaryObjectManager = BinaryObjectManager;
            _configuration = configuration;
             DBConnection = _configuration["ConnectionStrings:Default"];
            _dbContext = dbContext;
            _StatesRepository = StatesRepository;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);

        }

        #endregion

        [UnitOfWork]
        public override void Execute(ImportStatesJobArgs args)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                  
                    AsyncHelper.RunSync(() => CreateStatesUsingTemp());
                    
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }

        }
        public virtual async Task CreateStatesUsingTemp()
        {
            try
            {

                List<States> MasterColorList = new List<States>();

                var StatesTempFromExcel = await _db.QueryAsync<StatesTempRawData>("Select * from StatesTempRawData", new
                {

                }, commandType: System.Data.CommandType.Text);
                var CountryData = await _db.QueryAsync<Countries>("Select * from Countries", new
                {

                }, commandType: System.Data.CommandType.Text);
                var MasterBulkStates = await _db.QueryAsync<States>("Select * from States", new
                {

                }, commandType: System.Data.CommandType.Text);

                List<StatesTempRawData> distinctExcelCode = StatesTempFromExcel.Where(i => !string.IsNullOrEmpty(i.StateName)).ToList();
                List<StatesTempRawData> NewStatesToBeInserted = new List<StatesTempRawData>();
                List<States> FinalColorsList = new List<States>();
                List<States> FinalColorsToUpdate = new List<States>();
                List<States> FinalColorsToInsert = new List<States>();

                if (MasterBulkStates.ToList().Count() > 0)
                {
                    var dataToInsert = MasterBulkStates.Select(v => v.StateName.ToLower()).ToList().Except(StatesTempFromExcel.Select(b => b.StateName.ToLower()).ToList()).ToList();
                    var DataToUpdate = distinctExcelCode.Where(x => !dataToInsert.Contains(x.StateName)).ToList();
                    var DataToInsert = distinctExcelCode.Where(x => dataToInsert.Contains(x.StateName)).ToList();
                    NewStatesToBeInserted = distinctExcelCode.Where(x => dataToInsert.Contains(x.StateName)).ToList();

                    var DataToUpdateGroups = DataToUpdate.GroupBy(i => i.CountryISOCode.ToLower()).ToList();

                    foreach (var item in DataToUpdateGroups)
                    {
                        // if data exists in database
                        var CountryIsExists = CountryData.Where(i => i.ISO3Code.ToLower() == item.Key.ToLower()).FirstOrDefault();
                        if (CountryIsExists != null)
                        {
                            foreach (var value in item)
                            {
                                var UpdatedState = MasterBulkStates.ToList().Where(i => i.StateName == value.StateName && i.CountryId == CountryIsExists.Id).FirstOrDefault();
                                if (UpdatedState != null)
                                {
                                    UpdatedState.StateName = value.StateName;
                                    UpdatedState.CountryId = CountryIsExists.Id;
                                    UpdatedState.IsActive = true;
                                    FinalColorsToUpdate.Add(UpdatedState);
                                }
                                else
                                {
                                    States State = new States();
                                    if (CountryIsExists != null)
                                    {
                                        State.StateName = value.StateName;
                                        State.CountryId = CountryIsExists.Id;
                                        State.IsActive = true;
                                        FinalColorsToInsert.Add(State);
                                    }
                                }
                            }
                        }
                    }
                    if (FinalColorsToUpdate.Count > 0)
                    {
                        await UpdateStates(FinalColorsToUpdate);
                    }

                    var DataToInsertGroups = DataToInsert.GroupBy(i => i.CountryISOCode.ToLower()).ToList();

                    foreach (var key in DataToInsertGroups)
                    {
                        var CountryIsExists = CountryData.Where(i => i.ISO3Code.ToLower() == key.Key.ToLower()).FirstOrDefault();
                        if (CountryIsExists != null)
                        {
                            foreach (var value in key)
                            {
                                States State = new States();

                                if (CountryIsExists != null)
                                {
                                    State.StateName = value.StateName;
                                    State.CountryId = CountryIsExists.Id;
                                    State.IsActive = true;
                                    FinalColorsToInsert.Add(State);
                                }
                            }
                        }
                    }

                    if (FinalColorsToInsert.Count > 0)
                    {
                        await CreateStates(FinalColorsToInsert);
                    }
                }
                else
                {
                    // if no data exists in master table in database
                    var GroupInsertionData = distinctExcelCode.GroupBy(i => i.CountryISOCode.ToLower()).ToList();
                    foreach (var item in GroupInsertionData)
                    {
                        var CountryIsExists = CountryData.Where(i => i.ISO3Code.ToLower() == item.Key.ToLower()).FirstOrDefault();
                        if (CountryIsExists != null)
                        {
                            foreach (var value in item)
                            {
                                States State = new States();

                                if (CountryIsExists != null)
                                {
                                    State.StateName = value.StateName;
                                    State.CountryId = CountryIsExists.Id;
                                    State.IsActive = true;
                                    FinalColorsToInsert.Add(State);
                                }
                            }
                        }
                    }

                    if (FinalColorsToInsert.Count > 0)
                    {
                        await CreateStates(FinalColorsToInsert);
                    }
                }

                #region delete raw temp data

                await _db.QueryAsync<StatesTempRawData>("delete from StatesTempRawData DBCC CHECKIDENT ('StatesTempRawData', RESEED, 1)", new
                {

                }, commandType: System.Data.CommandType.Text);

                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateStates(List<States> StateList)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _StatesRepository.BulkInsertAsync(StateList);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task UpdateStates(List<States> StatesList)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _StatesRepository.BulkUpdate(StatesList);
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
