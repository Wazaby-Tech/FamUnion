using FamUnion.Core.Validation;
using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure
{
    public class DbAccess<T>
    {
        private string _connectionString;

        private static readonly CommandType _sProcType = CommandType.StoredProcedure;

        public DbAccess(string connectionString)
        {
            _connectionString = Validator.ThrowIfNull(connectionString, nameof(connectionString));
        }        

        protected async Task<IEnumerable<T>> ExecuteStoredProc(string proc, IDataMapper<T> mapper, ParameterDictionary parameters)
        {
            return await Execute(proc, mapper, parameters)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        protected async Task<IEnumerable<T>> ExecuteStoredProc(string proc, IDataMapper<T> mapper)
        {
            return await Execute(proc, mapper, null)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        protected async Task<IEnumerable<T>> ExecuteStoredProc(string proc, ParameterDictionary parameters)
        {
            return await Execute(proc, null, parameters)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        protected async Task<IEnumerable<T>> ExecuteStoredProc(string proc)
        {
            return await Execute(proc, null, null)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        private async Task<IEnumerable<T>> Execute(string proc, IDataMapper<T> mapper, ParameterDictionary parameters)
        {
            //_logger.LogInformation($"DataAccess.Execute|Stored Proc: {proc}, Return type: {typeof(T).ToString()}, Data Mapper: {mapper?.GetType()?.ToString() ?? "N/A"}, Parameters: {parameters?.GetDynamicObject() ?? "N/A"}");
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                if (mapper != null)
                {
                    SqlMapper.GridReader reader = await conn.QueryMultipleAsync(proc, parameters?.GetDynamicObject() ?? null, commandType: _sProcType)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    return await mapper.MapDataAsync(reader)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
                else if (parameters != null)
                {
                    return await conn.QueryAsync<T>(proc, parameters.GetDynamicObject(), commandType: _sProcType)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
                else
                {
                    return await conn.QueryAsync<T>(proc, commandType: _sProcType)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
            }
        }

        protected async Task<object> ExecuteScalar(string proc, ParameterDictionary parameters)
        {
            return await ExecuteScalarWithTimeout(proc, parameters, null)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        protected async Task<object> ExecuteScalarWithTimeout(string proc, ParameterDictionary parameters, int? timeout)
        {
            //_logger.LogInformation($"DataAccess.ExecuteScalarWithTimeout|Stored Proc: {proc}, Parameters: {parameters?.GetDynamicObject() ?? "N/A"}, Timeout: {timeout ?? 0}");
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                return await conn.ExecuteScalarAsync(proc, parameters?.GetDynamicObject(), commandTimeout: timeout, commandType: _sProcType)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }
    }
}
