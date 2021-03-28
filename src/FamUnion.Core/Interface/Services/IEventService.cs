using FamUnion.Core.Model;
using FamUnion.Core.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IEventService
    {
        Task<Event> GetEventByIdAsync(Guid eventId);
        Task<IEnumerable<Event>> GetEventsByReunionIdAsync(Guid eventId);
        Task<Event> SaveEventAsync(Event @event);
        Task CancelEventAsync(CancelRequest request);
    }
}
