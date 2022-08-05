using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Abp.Data;
using CaznerMarketplaceBackendApp.EntityFrameworkCore;
using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.Connections
{
    public class DbConnectionUtility : CaznerMarketplaceBackendAppAppServiceBase, IDbConnectionUtility
    {
        private CaznerMarketplaceBackendAppDbContext _dbContext;
        private readonly IActiveTransactionProvider _transactionProvider;
        public DbConnectionUtility(CaznerMarketplaceBackendAppDbContext dbContext, IActiveTransactionProvider transactionProvider)
        {
            _dbContext = dbContext;
            _transactionProvider = transactionProvider;
        }

        public DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {

            var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            // command.Transaction = GetActiveTransaction();

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }

        public async Task EnsureConnectionOpenAsync()
        {
            var connection = _dbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
        }

        private DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
                {
                {"ContextType", typeof(CaznerMarketplaceBackendAppDbContext) },
                {"MultiTenancySide", null }
                });
        }

    }
}
