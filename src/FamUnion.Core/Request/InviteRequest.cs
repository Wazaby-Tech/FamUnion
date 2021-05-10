using System;
using System.Collections.Generic;
using System.Text;

namespace FamUnion.Core.Request
{
    public class InviteRequest
    {
        public Guid ReunionId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class BulkInviteRequest
    {
        public IEnumerable<InviteRequest> InviteRequests { get; set; }
        public string UserId { get; set; }
    }
}
