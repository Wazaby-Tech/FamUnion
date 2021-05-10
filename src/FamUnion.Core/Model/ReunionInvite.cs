using System;
using System.Collections.Generic;
using System.Text;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Model
{
    public class ReunionInvite : ModelBase
    {
        public Guid ReunionId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int RsvpCount { get; set; } = 0;
        public DateTime ExpiresAt { get; set; }
        public InviteResponseStatus Status { get; set; } = InviteResponseStatus.NotResponded;

        public override bool IsValid()
        {
            return ReunionId != Guid.Empty
                && ExpiresAt != DateTime.MinValue
                && !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Email);
        }
    }
}
