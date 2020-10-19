using FamUnion.Core.Model;
using System;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(Guid userId);
        Task<User> SaveUserAsync(User user);
    }
}
