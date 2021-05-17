using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IUserRepository
    {
        Task<bool> ValidateUserIdAsync(string userId);
        Task<bool> ValidateEmailAsync(string email);
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> SaveUserAsync(User user);
        Task<IEnumerable<User>> GetReunionOrganizers(Guid reunionId);
    }
}
