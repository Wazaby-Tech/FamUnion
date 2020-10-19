using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace FamUnion.Core.Model
{
    public interface IAuditInfo
    {
        [MaxLength(100)]
        [JsonIgnore]
        string CreatedBy { get; set; }
        [JsonIgnore]
        DateTime? CreatedDate { get; set; }
        [MaxLength(100)]
        [JsonIgnore]
        string ModifiedBy { get; set; }
        [JsonIgnore]
        DateTime? ModifiedDate { get; set; }
     }
}
