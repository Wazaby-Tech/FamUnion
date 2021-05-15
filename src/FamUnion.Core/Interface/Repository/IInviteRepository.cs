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
        Task<IEnumerable<ReunionInvite>> GetInvitesByReunion(Guid reunionId);
        Task<ReunionInvite> GetInviteAsync(InviteInfo inviteInfo);
        Task InviteResponseAsync(Guid inviteId, InviteResponseStatus status);
        Task CreateInvites(BulkInviteRequest request);
    }
}
