using FamUnion.Core.Model;
using FamUnion.Core.Request;
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
        Task CancelReunionAsync(CancelRequest request);

        Task AddReunionOrganizer(OrganizerRequest request);
        Task RemoveReunionOrganizer(OrganizerRequest request);
        Task<IEnumerable<User>> GetReunionOrganizers(OrganizerRequest request);
    }
}
