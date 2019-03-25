using FamUnion.Core.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Model
{
    public class FamilyMember : ModelBase
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Age => DateOfBirth.Age();
        public Size TshirtSize { get; set; }
        public string PhotoLocation { get; set; }

        public virtual User UserAccount { get; set; }
        public virtual Family Family { get; set; }

    }
}
