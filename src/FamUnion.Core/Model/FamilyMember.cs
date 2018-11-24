using FamUnion.Core.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Model
{
    public class FamilyMember : IAuditInfo
    {
        [Key]
        public Guid FamilyMemberId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Age => DateOfBirth.HasValue ? DateOfBirth.Value.Age() : -1;
        public GenderIdentity Gender { get; set; }
        public Size TshirtSize { get; set; }
        public string PhotoLocation { get; set; }

        public virtual User UserAccount { get; set; }
        public virtual Family Family { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
