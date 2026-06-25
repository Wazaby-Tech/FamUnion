using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
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
            const string sql = "SELECT * FROM sp_get_event_by_id(@id)";
            return (await ExecuteStoredProc(sql, ParameterDictionary.Single("id", eventId))
                .ConfigureAwait(false)).SingleOrDefault();
        }

        public async Task<IEnumerable<Event>> GetEventsByReunionIdAsync(Guid reunionId)
        {
            const string sql = "SELECT * FROM sp_get_events_by_reunion_id(@reunionId)";
            return await ExecuteStoredProc(sql, ParameterDictionary.Single("reunionId", reunionId))
                .ConfigureAwait(false);
        }

        public async Task<Event> SaveEventAsync(Event @event)
        {
            const string sql = "SELECT * FROM sp_save_event(@userId, @id, @reunionId, @name, @details, @startTime, @endTime, @attireType)";
            ParameterDictionary parameters = new ParameterDictionary(
                "userId",      @event.ActionUserId,
                "id",          @event.Id ?? Guid.NewGuid(),
                "reunionId",   @event.ReunionId,
                "name",        @event.Name,
                "details",     @event.Details,
                "startTime",   @event.StartTime.UtcDateTime,
                "endTime",     @event.EndTime?.UtcDateTime,
                "attireType",  (int)@event.AttireType
            );

            return (await ExecuteStoredProc(sql, parameters).ConfigureAwait(false)).SingleOrDefault();
        }

        public async Task CancelEventAsync(CancelRequest request)
        {
            const string sql = "SELECT sp_cancel_event_by_id(@userId, @eventId)";
            ParameterDictionary parameters = new ParameterDictionary(
                "userId",  request.UserId,
                "eventId", request.EntityId
            );
            await ExecuteNonQueryProc(sql, parameters).ConfigureAwait(false);
        }
    }
}
