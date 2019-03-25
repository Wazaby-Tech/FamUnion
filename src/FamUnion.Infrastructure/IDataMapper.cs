using Dapper;
using System.Collections.Generic;

namespace FamUnion.Infrastructure
{
    public interface IDataMapper<T>
    {
        IEnumerable<T> MapData(SqlMapper.GridReader reader);
    }
}
