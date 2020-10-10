using System;

namespace FamUnion.Core.Interface
{
    public abstract class RequestMapperBase<T,C>
    {
        public static T Map(C request)
        {
            throw new NotImplementedException();
        }
    }
}
