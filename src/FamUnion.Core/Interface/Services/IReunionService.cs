using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IReunionService
    {
        Task<IEnumerable<Reunion>> GetReunionsAsync();
        Task<IEnumerable<Reunion>> GetManageReunionsAsync(string userId);
        Task<Reunion> GetReunionAsync(Guid id);
        Task<Reunion> SaveReunionAsync(Reunion reunion);
        Task DeleteReunionAsync(Guid id);

        Task AddReunionOrganizer(Guid reunionId, string userId);
        Task RemoveReunionOrganizer(Guid reunion, string userId);
    }
}
