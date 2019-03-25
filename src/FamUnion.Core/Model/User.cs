using Microsoft.AspNetCore.Identity;
using System;

namespace FamUnion.Core.Model
{
    public class User : IdentityUser, IAuditInfo
    {
        public Guid UserId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        //public virtual IEnumerable<Reunion> Reunions { get; set; }
        public virtual Family Family { get; set; }
    }
}
