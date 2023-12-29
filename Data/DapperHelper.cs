using Dapper;
using Data.Abtract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DapperHelper : IDapperHelper
    {
        private readonly string connectString = string.Empty;
        private IDbConnection _dbConnection;

        public DapperHelper(IConfiguration configuration)
        {
            connectString = configuration.GetConnectionString("SqlServer");
            _dbConnection = new SqlConnection(connectString);
        }

        public async Task ExecuteNotReturn(
            string query, 
            DynamicParameters parameters = null, 
            IDbTransaction dbTransaction = null)
        {
            using (var dbConn = new SqlConnection(connectString)) 
            {
                await dbConn.ExecuteAsync(
                    query, 
                    parameters, 
                    dbTransaction, 
                    commandType: CommandType.Text);
            }
        }

        public async Task<T> ExecuteReturnFirst<T>(
            string query,
            DynamicParameters parameters = null,
            IDbTransaction dbTransaction = null)
        {
            using (var dbConn = new SqlConnection(connectString))
            {
                return await dbConn.QueryFirstAsync<T>(
                    query,
                    parameters,
                    dbTransaction,
                    commandType: CommandType.Text);

            }
        }

        public async Task<T> ExecuteReturnScalar<T>(
            string query, 
            DynamicParameters parameters = null,
            IDbTransaction dbTransaction = null)
        {
            using (var dbConn = new SqlConnection(connectString))
            {
                return await dbConn.ExecuteScalarAsync<T>(
                    query,
                    parameters,
                    dbTransaction,
                    commandType: CommandType.Text);

            }
        }


        public async Task<IEnumerable<T>> ExecuteSqlGetList<T>(
            string query, 
            DynamicParameters parameters = null,
            IDbTransaction dbTransaction = null)
        {
            using (var dbConn = new SqlConnection(connectString))
            {
                return await dbConn.QueryAsync<T>(
                    query,
                    parameters, 
                    dbTransaction, 
                    commandType: CommandType.Text);
            }
        }

        public async Task<IEnumerable<T>> ExecuteProcGetList<T>(
            string query, 
            DynamicParameters parameters = null,
            IDbTransaction dbTransaction = null)
        {
            using (var dbConn = new SqlConnection(connectString))
            {
                return await dbConn.QueryAsync<T>(
                    query, 
                    parameters, 
                    dbTransaction,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }

}
