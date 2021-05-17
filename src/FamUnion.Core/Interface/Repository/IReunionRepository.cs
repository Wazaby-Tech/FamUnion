using FamUnion.Core.Model;
using FamUnion.Core.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IReunionRepository
    {
        Task<IEnumerable<Reunion>> GetReunionsAsync();
        Task<IEnumerable<Reunion>> GetManageReunionsAsync(string userId);
        Task<Reunion> GetReunionAsync(Guid id);
        Task<Reunion> SaveReunionAsync(Reunion reunion);
        Task CancelReunionAsync(CancelRequest request);

        Task AddReunionOrganizer(Guid reunionId, string email);
        Task RemoveReunionOrganizer(Guid reunionId, string email);
    }
}
