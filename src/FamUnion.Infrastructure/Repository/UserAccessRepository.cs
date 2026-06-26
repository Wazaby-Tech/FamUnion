using FamUnion.Core.Interface.Repository;
using FamUnion.Core.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure.Repository
{
    public class UserAccessRepository : DbAccess<bool>, IUserAccessRepository
    {
        public UserAccessRepository(string connection)
            : base(connection)
        {
        }

        public async Task<bool> HasReadAccessToEntity(string userId, Constants.EntityType type, Guid id)
        {
            const string sql = "SELECT result FROM sp_user_has_read_access_to_entity(@userId, @entityType, @entityId)";
            ParameterDictionary parameters = new ParameterDictionary(
                "userId",     userId,
                "entityType", (int)type,
                "entityId",   id
            );
            return (await ExecuteStoredProc(sql, parameters).ConfigureAwait(false)).FirstOrDefault();
        }

        public async Task<bool> HasWriteAccessToEntity(string userId, Constants.EntityType type, Guid id)
        {
            const string sql = "SELECT result FROM sp_user_has_write_access_to_entity(@userId, @entityType, @entityId)";
            ParameterDictionary parameters = new ParameterDictionary(
                "userId",     userId,
                "entityType", (int)type,
                "entityId",   id
            );
            return (await ExecuteStoredProc(sql, parameters).ConfigureAwait(false)).FirstOrDefault();
        }
    }
}
