using Dapper;
using FamUnion.Core.Validation;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FamUnion.Infrastructure
{
    public abstract class DataAccess
    {
        protected string _connectionString;
        protected static object[] _emptyParams = new object[] { };
        protected readonly ILogger _logger;

        private static readonly CommandType _sProcType = CommandType.StoredProcedure;

        protected DataAccess(string connection, ILogger logger)
        {            
            _connectionString = Validator.ThrowIfNull(connection, nameof(connection));
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
        }

        protected IEnumerable<T> ExecuteStoredProc<T>(string proc, IDataMapper<T> mapper, ParameterDictionary parameters)
        {
            return Execute<T>(proc, mapper, parameters);
        }

        protected IEnumerable<T> ExecuteStoredProc<T>(string proc, IDataMapper<T> mapper)
        {
            return Execute<T>(proc, mapper, null);
        }

        protected IEnumerable<T> ExecuteStoredProc<T>(string proc, ParameterDictionary parameters)
        {
            return Execute<T>(proc, null, parameters);
        }

        protected IEnumerable<T> ExecuteStoredProc<T>(string proc)
        {
            return Execute<T>(proc, null, null);
        }

        private IEnumerable<T> Execute<T>(string proc, IDataMapper<T> mapper, ParameterDictionary parameters)
        {
            _logger.LogInformation($"DataAccess.Execute|Stored Proc: {proc}, Return type: {typeof(T).ToString()}, Data Mapper: {mapper?.GetType()?.ToString() ?? "N/A"}, Parameters: {parameters?.GetDynamicObject() ?? "N/A"}");
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                if (mapper != null)
                {
                    SqlMapper.GridReader reader = conn.QueryMultiple(proc, parameters?.GetDynamicObject() ?? null, commandType: _sProcType);
                    return mapper.MapData(reader);
                }
                else if (parameters != null)
                {
                    IEnumerable<T> result = conn.Query<T>(proc, parameters.GetDynamicObject(), commandType: _sProcType);
                    return result;
                }
                else
                {
                    IEnumerable<T> result = conn.Query<T>(proc, commandType: _sProcType);
                    return result;
                }
            }
        }

        protected object ExecuteScalar(string proc, ParameterDictionary parameters)
        {
            return ExecuteScalarWithTimeout(proc, parameters, null);
        }
        
        protected object ExecuteScalarWithTimeout(string proc, ParameterDictionary parameters, int? timeout)
        {
            _logger.LogInformation($"DataAccess.ExecuteScalarWithTimeout|Stored Proc: {proc}, Parameters: {parameters?.GetDynamicObject() ?? "N/A"}, Timeout: {timeout ?? 0}");
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                return conn.ExecuteScalar(proc, parameters?.GetDynamicObject(), commandTimeout: timeout, commandType: _sProcType);
            }
        }
    }
}
