using System;
using System.Collections.Generic;
using System.Dynamic;

namespace FamUnion.Core.Utility
{
    public static class Extensions
    {
        public static int? Age(this DateTime dob)
        {
            var now = DateTime.Today;
            var age = now.Year - dob.Year - 1;
            age += (now.Month <= dob.Month) ? (now.Month == dob.Month && now.Day >= dob.Day ? 1 : 0) : 1;
            return age;
        }

        public static int? Age(this DateTime? dob)
        {
            if (!dob.HasValue)
                return null;
            return dob.Value.Age();
        }

        public static dynamic ToDynamic<T>(this T obj)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var currentValue = propertyInfo.GetValue(obj);
                expando.Add(propertyInfo.Name, currentValue);
            }
            return expando as ExpandoObject;
        }

        public static string GetDbGuidString(this Guid? id)
        {
            return (id ?? Guid.Empty) == Guid.Empty ? Guid.NewGuid().ToString() : id.ToString();
        }

        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}
