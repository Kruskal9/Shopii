using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Abtract
{
    public interface IDapperHelper
    {
        Task ExecuteNotReturn(
            string query, 
            DynamicParameters parameters = null, 
            IDbTransaction dbTransaction = null);
        Task<IEnumerable<T>> ExecuteProcGetList<T>(
            string query, 
            DynamicParameters parameters = null, 
            IDbTransaction dbTransaction = null);
        Task<T> ExecuteReturnFirst<T>(
            string query, 
            DynamicParameters parameters = null, 
            IDbTransaction dbTransaction = null);
        Task<T> ExecuteReturnScalar<T>(
            string query, 
            DynamicParameters parameters = null, 
            IDbTransaction dbTransaction = null);
        Task<IEnumerable<T>> ExecuteSqlGetList<T>(
            string query, 
            DynamicParameters parameters = null, 
            IDbTransaction dbTransaction = null);
    }
}
