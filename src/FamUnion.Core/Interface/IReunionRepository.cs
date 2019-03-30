using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IReunionRepository
    {
        Task<IEnumerable<Reunion>> GetReunions();
        Task<Reunion> GetReunion(Guid id);
        Task<Reunion> SaveReunion(Reunion reunion);
    }
}
