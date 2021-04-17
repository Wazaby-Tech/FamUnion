using System;
using System.Threading.Tasks;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Interface.Repository
{
    public interface IUserAccessRepository
    {
        Task<bool> HasReadAccessToEntity(string userId, EntityType type, Guid id);
        Task<bool> HasWriteAccessToEntity(string userId, EntityType type, Guid id);
    }
}
