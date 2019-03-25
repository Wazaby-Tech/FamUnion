using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamUnion.Core.Interface
{
    public interface IReunionService
    {
        IEnumerable<Reunion> GetReunions();
        Reunion GetReunion(Guid id);
        Reunion SaveReunion(Reunion reunion);
    }
}
