using System;
using System.ComponentModel.DataAnnotations;

namespace FamUnion.Core.Model
{
    public class Event : IAuditInfo
    {
        [Key]
        public Guid EventId { get; set; }
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

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
