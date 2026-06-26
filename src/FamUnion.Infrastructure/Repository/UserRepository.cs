using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure.Repository
{
    public class UserRepository : DbAccess<User>, IUserRepository
    {
        public UserRepository(string connection)
            : base(connection)
        {
        }

        public async Task<bool> ValidateUserIdAsync(string userId)
        {
            return (await GetUserByIdAsync(userId).ConfigureAwait(false)) != null;
        }

        public async Task<bool> ValidateEmailAsync(string email)
        {
            return (await GetUserByEmailAsync(email).ConfigureAwait(false)) != null;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            const string sql = "SELECT * FROM sp_get_user_by_id(@userId)";
            return (await ExecuteStoredProc(sql, ParameterDictionary.Single("userId", userId))
                .ConfigureAwait(false)).FirstOrDefault();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            const string sql = "SELECT * FROM sp_get_user_by_email(@email)";
            return (await ExecuteStoredProc(sql, ParameterDictionary.Single("email", email))
                .ConfigureAwait(false)).FirstOrDefault();
        }

        public async Task<User> SaveUserAsync(User user)
        {
            const string sql = "SELECT * FROM sp_save_user(@id, @userId, @email, @firstName, @lastName, @authType)";
            ParameterDictionary parameters = new ParameterDictionary(
                "id",        user.Id ?? Guid.NewGuid(),
                "userId",    user.UserId,
                "email",     user.Email,
                "firstName", user.FirstName,
                "lastName",  user.LastName,
                "authType",  (int)user.AuthType
            );

            return (await ExecuteStoredProc(sql, parameters).ConfigureAwait(false)).SingleOrDefault();
        }

        public async Task<IEnumerable<User>> GetReunionOrganizers(Guid reunionId)
        {
            const string sql = "SELECT * FROM sp_get_organizers_by_reunion_id(@reunionId)";
            return await ExecuteStoredProc(sql, ParameterDictionary.Single("reunionId", reunionId))
                .ConfigureAwait(false);
        }
    }
}
