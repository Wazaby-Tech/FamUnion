using FamUnion.Core.Model;
using FamUnion.Core.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IEventRepository
    {
        Task<Event> GetEventAsync(Guid eventId);
        Task<IEnumerable<Event>> GetEventsByReunionIdAsync(Guid reunionId);
        Task<Event> SaveEventAsync(Event @event);
        Task CancelEventAsync(CancelRequest request);
    }
}
