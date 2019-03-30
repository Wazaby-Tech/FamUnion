using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure
{
    public interface IDataMapper<T>
    {
        IEnumerable<T> MapData(SqlMapper.GridReader reader);
        Task<IEnumerable<T>> MapDataAsync(SqlMapper.GridReader reader);
    }
}
