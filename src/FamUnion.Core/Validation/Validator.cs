using System;
using System.Collections.Generic;
using System.Linq;

namespace FamUnion.Core.Validation
{
    public class Validator
    {
        public static T ThrowIfNull<T>(T value, string name)
        {
            if(value == null)
            {
                throw new ArgumentNullException(name);
            }

            return value;
        }

        public static ICollection<T> ThrowIfNullOrEmpty<T>(ICollection<T> values, string name)
        {
            if(values == null || !values.Any())
            {
                throw new ArgumentNullException(name);
            }

            return values;
        }
    }
}
