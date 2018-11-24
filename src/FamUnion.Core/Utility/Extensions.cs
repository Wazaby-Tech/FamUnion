using System;

namespace FamUnion.Core.Utility
{
    public static class Extensions
    {
        public static int Age(this DateTime dob)
        {
            var now = DateTime.Now;
            var age = now.Year - dob.Year - 1;
            age += (now.Month <= dob.Month) ? (now.Month == dob.Month && now.Day >= dob.Day ? 1 : 0) : 1;
            return age;
        }
    }
}
