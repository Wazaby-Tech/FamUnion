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
        public double? Duration { get; set; }
        public Guid AddressId { get; set; }

        public virtual Reunion Reunion { get; set; }
        public virtual Address Location { get; set; }

    }
}
