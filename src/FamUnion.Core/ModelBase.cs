using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;

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
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (var propertyInfo in GetType().GetProperties())
            {
                var currentValue = propertyInfo.GetValue(this);
                expando.Add(propertyInfo.Name, currentValue);
            }
            return expando as ExpandoObject;
        }
    }
}
