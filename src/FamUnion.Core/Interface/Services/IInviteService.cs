using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Interface.Services
{
    public interface IInviteService
    {
        Task<ReunionInvite> GetInviteAsync(InviteInfo inviteInfo);
        Task InviteResponseAsync(Guid inviteId, InviteResponseStatus status);
        Task CreateInvites(BulkInviteRequest request);
    }
}
