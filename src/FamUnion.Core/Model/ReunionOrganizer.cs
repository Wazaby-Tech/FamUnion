using System;

namespace FamUnion.Core.Model
{
    public class ReunionOrganizer
    {
        public Guid ReunionId { get; set; }
        public Guid UserId { get; set; }

        public virtual Reunion Reunion { get; set; }
    }
}
