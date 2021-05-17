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
        public AttendeeResponseStatus Status { get; set; } = AttendeeResponseStatus.NotSent;

        public override bool IsValid()
        {
            return ReunionId != Guid.Empty
                && !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Email);
        }
    }
}
