using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FamUnion.Core.Model
{
    public class Reunion : ModelBase
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public virtual Address Location { get; set; }
        public virtual IEnumerable<Event> Events { get; set; }
        public virtual IEnumerable<User> Organizers { get; set; }
        public virtual IEnumerable<Lodging> Lodgings { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Name)
                && (StartDate ?? DateTime.MinValue) < (EndDate ?? DateTime.MaxValue);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
