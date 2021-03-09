using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Utility;
using System;
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

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return (await ExecuteStoredProc("[dbo].[spGetUserBydId]", ParameterDictionary.Single("id", userId))
                .ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return (await ExecuteStoredProc("[dbo].[spGetUserByEmail]", ParameterDictionary.Single("email", email))
                .ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault();
        }

        public async Task<User> SaveUserAsync(User user)
        {
            ParameterDictionary parameters = new ParameterDictionary(new string[] {
                "id", user.Id.GetDbGuidString(),
                "userId", user.UserId,
                "email", user.Email,
                "firstName", user.FirstName,
                "lastName", user.LastName
            });

            return (await ExecuteStoredProc("[dbo].[spSaveUser]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault();
        }
    }
}
