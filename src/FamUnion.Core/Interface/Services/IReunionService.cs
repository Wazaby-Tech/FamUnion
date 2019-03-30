using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IReunionService
    {
        Task<IEnumerable<Reunion>> GetReunions();
        Task<Reunion> GetReunion(Guid id);
        Task<Reunion> SaveReunion(Reunion reunion);
    }
}
