using System;
using System.Collections.Generic;
using System.Text;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Model
{
    public class ReunionInvite
    {
        public Guid ReunionId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public InviteResponseStatus Status { get; set; } = InviteResponseStatus.NotResponded;
    }
}
