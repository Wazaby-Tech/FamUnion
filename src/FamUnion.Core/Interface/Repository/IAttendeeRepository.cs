using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Interface.Repository
{
    public interface IAttendeeRepository
    {
        Task<IEnumerable<AttendeeInvite>> GetAttendeesByReunion(Guid reunionId);
        Task<AttendeeInvite> GetInviteAsync(InviteInfo inviteInfo);
        Task AttendeeResponseAsync(Guid attendeeId, AttendeeResponseStatus status);
        Task AddAttendees(BulkAttendeeRequest request);
    }
}
