using FamUnion.Core.Validation;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure
{
    public class DbAccess<T>
    {
        private readonly string _connectionString;

        public DbAccess(string connectionString)
        {
            _connectionString = Validator.ThrowIfNull(connectionString, nameof(connectionString));
        }

        protected async Task<IEnumerable<T>> ExecuteStoredProc(string sql, IDataMapper<T> mapper, ParameterDictionary parameters)
        {
            return await Execute(sql, mapper, parameters).ConfigureAwait(false);
        }

        protected async Task<IEnumerable<T>> ExecuteStoredProc(string sql, IDataMapper<T> mapper)
        {
            return await Execute(sql, mapper, null).ConfigureAwait(false);
        }

        protected async Task<IEnumerable<T>> ExecuteStoredProc(string sql, ParameterDictionary parameters)
        {
            return await Execute(sql, null, parameters).ConfigureAwait(false);
        }

        protected async Task<IEnumerable<T>> ExecuteStoredProc(string sql)
        {
            return await Execute(sql, null, null).ConfigureAwait(false);
        }

        protected async Task ExecuteNonQueryProc(string sql, ParameterDictionary parameters)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync().ConfigureAwait(false);
            await conn.ExecuteAsync(sql, parameters?.GetDynamicObject(), commandType: CommandType.Text)
                .ConfigureAwait(false);
        }

        protected async Task ExecuteNonQueryProc(string sql)
        {
            await ExecuteNonQueryProc(sql, null).ConfigureAwait(false);
        }

        private async Task<IEnumerable<T>> Execute(string sql, IDataMapper<T> mapper, ParameterDictionary parameters)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync().ConfigureAwait(false);

            if (mapper != null)
            {
                SqlMapper.GridReader reader = await conn.QueryMultipleAsync(sql, parameters?.GetDynamicObject(), commandType: CommandType.Text)
                    .ConfigureAwait(false);
                return await mapper.MapDataAsync(reader).ConfigureAwait(false);
            }

            return await conn.QueryAsync<T>(sql, parameters?.GetDynamicObject(), commandType: CommandType.Text)
                .ConfigureAwait(false);
        }

        protected async Task<object> ExecuteScalar(string sql, ParameterDictionary parameters)
        {
            return await ExecuteScalarWithTimeout(sql, parameters, null).ConfigureAwait(false);
        }

        protected async Task<object> ExecuteScalarWithTimeout(string sql, ParameterDictionary parameters, int? timeout)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync().ConfigureAwait(false);
            return await conn.ExecuteScalarAsync(sql, parameters?.GetDynamicObject(), commandTimeout: timeout, commandType: CommandType.Text)
                .ConfigureAwait(false);
        }
    }
}
