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
        
        public virtual Address CityLocation { get; set; }
        public virtual IEnumerable<Event> Events { get; set; }
        public virtual IEnumerable<User> Organizers { get; set; }
        public virtual IEnumerable<Family> Families { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
