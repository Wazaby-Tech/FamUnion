using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure.Repository
{
    public class EventRepository : DbAccess<Event>, IEventRepository
    {
        public EventRepository(string connection)
            : base(connection)
        {

        }

        public Task<IEnumerable<Event>> GetEventsByReunionAsync(Guid reunionId)
        {
            throw new NotImplementedException();
        }

        public Task<Event> SaveEventAsync(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
