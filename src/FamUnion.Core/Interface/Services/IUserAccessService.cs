using FamUnion.Core.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface.Services
{
    public interface IUserAccessService
    {
        Task<bool> HasReadAccessToEntity(string userId, Constants.EntityType type, Guid id);
        Task<bool> HasWriteAccessToEntity(string userId, Constants.EntityType type, Guid id);
    }
}
