using FamUnion.Core.Model;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> SaveUserAsync(User user);
    }
}
