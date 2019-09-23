using System;
using System.ComponentModel.DataAnnotations;

namespace FamUnion.Core.Model
{
    public class Event : ModelBase
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(2000)]
        public string Details { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public TimeSpan? Duration => EndTime.HasValue ? EndTime - StartTime : null;
        public Guid ReunionId { get; set; }

        public virtual Address Location { get; set; }

        public override bool IsValid()
        {
            bool validTimes = EndTime.HasValue ? EndTime > StartTime : true;
            bool validLocation = Location != null ? Location.IsValid() : true;
            return !string.IsNullOrEmpty(Name)
                && validTimes
                && validLocation;
        }
    }
}
