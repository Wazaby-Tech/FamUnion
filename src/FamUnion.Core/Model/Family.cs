using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FamUnion.Core.Model
{
    public class Family : ModelBase
    {
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }        
        public bool Attending { get; set; }

        public virtual IEnumerable<FamilyMember> Members { get; set; }
        public virtual Address Address { get; set; }
    }
}
