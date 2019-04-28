using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetEventsByReunionAsync(Guid reunionId);
        Task<Event> SaveEventAsync(Event @event);
    }
}
