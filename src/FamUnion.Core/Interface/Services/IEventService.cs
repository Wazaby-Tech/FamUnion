using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IEventService
    {
        Task<Event> GetEventAsync(Guid eventId);
        Task<IEnumerable<Event>> GetEventsByReunionAsync(Guid eventId);
        Task<Event> SaveEventAsync(Event @event);
    }
}
