using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Interface.Repository
{
    public interface IInviteRepository
    {
        Task<IEnumerable<AttendeeInvite>> GetInvitesByReunion(Guid reunionId);
        Task<AttendeeInvite> GetInviteAsync(InviteInfo inviteInfo);
        Task InviteResponseAsync(Guid inviteId, InviteResponseStatus status);
        Task CreateInvites(BulkInviteRequest request);
    }
}
