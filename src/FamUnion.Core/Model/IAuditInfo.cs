using System;
using System.ComponentModel.DataAnnotations;

namespace FamUnion.Core.Model
{
    public interface IAuditInfo
    {
        [MaxLength(100)]
        string CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        [MaxLength(100)]
        string ModifiedBy { get; set; }
        DateTime ModifiedDate { get; set; }
     }
}
