using Auth0.ManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamUnion.Auth
{
    public interface IAuthenticateService
    {
        Task<IList<User>> GetUserByEmailAsync(string email);
        Task CreateUserAsync(string email, string password);
        Task CreateFacebookUserAsync(string email);
        void AuthenticateUserAsync();
    }
}
