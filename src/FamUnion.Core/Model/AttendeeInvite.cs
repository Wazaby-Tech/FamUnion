using System;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Model
{
    public class AttendeeInvite : ModelBase
    {
        public Guid ReunionId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int RsvpCount { get; set; } = 0;
        public DateTime? ExpiresAt { get; set; }
        public bool InviteSent { get; set; } = false;
        public InviteResponseStatus Status { get; set; } = InviteResponseStatus.NotResponded;

        public override bool IsValid()
        {
            return ReunionId != Guid.Empty
                && !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Email);
        }
    }
}
