using FamUnion.Core.Interface.Repository;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure.Repository
{
    public class AttendeeRepository : DbAccess<AttendeeInvite>, IAttendeeRepository
    {
        public AttendeeRepository(string connection)
            : base(connection)
        {
        }

        public async Task<IEnumerable<AttendeeInvite>> GetAttendeesByReunion(Guid reunionId)
        {
            const string sql = "SELECT * FROM sp_get_invites_by_reunion_id(@reunionId)";
            return await ExecuteStoredProc(sql, ParameterDictionary.Single("reunionId", reunionId))
                .ConfigureAwait(false);
        }

        public async Task AddAttendees(BulkAttendeeRequest request)
        {
            const string sql = "SELECT sp_create_invites(@invites, @userId)";
            ParameterDictionary parameters = ParameterDictionary.Single("invites", TvpHelper.MapInvites(request.InviteRequests));
            parameters.AddParameter("userId", request.UserId);
            await ExecuteNonQueryProc(sql, parameters).ConfigureAwait(false);
        }

        public async Task<AttendeeInvite> GetInviteAsync(InviteInfo inviteInfo)
        {
            const string sql = "SELECT * FROM sp_get_invite_by_id(@inviteId)";
            return (await ExecuteStoredProc(sql, ParameterDictionary.Single("inviteId", inviteInfo.InviteId))
                .ConfigureAwait(false)).FirstOrDefault();
        }

        public async Task AttendeeResponseAsync(Guid inviteId, Constants.AttendeeResponseStatus status)
        {
            const string sql = "SELECT sp_update_invite_status(@inviteId, @status)";
            ParameterDictionary parameters = new ParameterDictionary(
                "inviteId", inviteId,
                "status",   (int)status
            );
            await ExecuteNonQueryProc(sql, parameters).ConfigureAwait(false);
        }
    }
}
