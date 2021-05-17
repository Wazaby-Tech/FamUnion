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
            ParameterDictionary parameters = ParameterDictionary.Single("reunionId", reunionId);
            return await ExecuteStoredProc("[dbo].[spGetInvitesByReunionId]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task AddAttendees(BulkAttendeeRequest request)
        {
            ParameterDictionary parameters = ParameterDictionary.Single("invites", TvpHelper.MapInvites(request.InviteRequests));
            parameters.AddParameter("userId", request.UserId);

            await ExecuteStoredProc("[dbo].[spCreateInvites]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<AttendeeInvite> GetInviteAsync(InviteInfo inviteInfo)
        {
            ParameterDictionary parameters = ParameterDictionary.Single("inviteId", inviteInfo.InviteId);

            return (await ExecuteStoredProc("[dbo].[spGetInviteById]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault();
        }

        public async Task AttendeeResponseAsync(Guid inviteId, Constants.AttendeeResponseStatus status)
        {
            ParameterDictionary parameters = new ParameterDictionary();
            parameters.AddParameter("inviteId", inviteId);
            parameters.AddParameter("status", (int)status);

            await ExecuteStoredProc("[dbo].[spUpdateStatus]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
