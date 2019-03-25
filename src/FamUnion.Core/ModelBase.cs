using FamUnion.Core.Model;
using System;

namespace FamUnion.Core
{
    public abstract class ModelBase : IAuditInfo
    {
        public Guid? Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual bool IsValid()
        {
            return true;
        }

        public virtual dynamic ToDynamic()
        {
            return new { };
        }
    }
}
