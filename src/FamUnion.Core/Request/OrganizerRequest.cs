using System;
using System.Collections.Generic;
using System.Text;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Request
{
    public class OrganizerRequest
    {
        public Guid ReunionId { get; set; }
        public OrganizerAction Action { get; set; }
        public string Email { get; set; }
        public string ActionUserId { get; set; }

        public static OrganizerRequest AddOrganizerRequest(Guid reunionId, string email, string actionUserId)
        {
            return new OrganizerRequest()
            {
                ReunionId = reunionId,
                Action = OrganizerAction.Add,
                Email = email,
                ActionUserId = actionUserId
            };
        }

        public static OrganizerRequest RemoveOrganizerRequest(Guid reunionId, string email, string actionUserId)
        {
            return new OrganizerRequest()
            {
                ReunionId = reunionId,
                Action = OrganizerAction.Remove,
                Email = email,
                ActionUserId = actionUserId
            };
        }
    }
}
