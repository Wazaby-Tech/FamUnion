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

        public Task<bool> HasReadAccessToEntity(string userId, Constants.EntityType type, Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> HasWriteAccessToEntity(string userId, Constants.EntityType type, Guid id)
        {
            ParameterDictionary parameters = new ParameterDictionary(new string[]
            {
                "userId", userId,
                "entityType", ((int)type).ToString(),
                "entityId", id.ToString()
            });

            var result = (await ExecuteStoredProc("[dbo].[spUserHasWriteAccessToEntity]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault();
            return result;
        }
    }
}
