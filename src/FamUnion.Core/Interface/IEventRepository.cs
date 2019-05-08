using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IEventRepository
    {
        Task<Event> GetEventAsync(Guid eventId);
        Task<IEnumerable<Event>> GetEventsByReunionAsync(Guid reunionId);
        Task<Event> SaveEventAsync(Event @event);
    }
}
