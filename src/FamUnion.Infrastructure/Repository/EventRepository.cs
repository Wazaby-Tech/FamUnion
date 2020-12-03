using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Event> GetEventAsync(Guid eventId)
        {
            ParameterDictionary parameters = ParameterDictionary.Single("id", eventId.ToString());
            return (await ExecuteStoredProc("[dbo].[spGetEventById]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault();
        }

        public async Task<IEnumerable<Event>> GetEventsByReunionIdAsync(Guid reunionId)
        {
            ParameterDictionary parameters = ParameterDictionary.Single("reunionId", reunionId.ToString());
            return await ExecuteStoredProc("[dbo].[spGetEventsByReunionId]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<Event> SaveEventAsync(Event @event)
        {
            ParameterDictionary parameters = new ParameterDictionary(new string[] {
                "id", @event.Id.GetDbGuidString(),
                "reunionId", @event.ReunionId.ToString(),
                "name", @event.Name,
                "details", @event.Details,
                "startTime", @event.StartTime.ToString(),
                "endTime", @event.EndTime.ToString()
            });

            return (await ExecuteStoredProc("[dbo].[spSaveEvent]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault();
        }

        public async Task DeleteEventAsync(Guid eventId)
        {
            ParameterDictionary parameters = ParameterDictionary.Single("eventId", eventId.ToString());

            await ExecuteStoredProc("[dbo].[spDeleteEventById]", parameters).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
