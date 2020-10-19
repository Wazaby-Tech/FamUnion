using FamUnion.Core.Model;
using Newtonsoft.Json;
using System;

namespace FamUnion.Core
{
    public abstract class ModelBase : IAuditInfo
    {
        public Guid? Id { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
        [JsonIgnore]
        public string ModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime? ModifiedDate { get; set; }

        public virtual bool IsValid()
        {
            return true;
        }
    }
}
