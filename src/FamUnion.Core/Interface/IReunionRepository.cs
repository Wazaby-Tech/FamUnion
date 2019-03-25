using FamUnion.Core.Model;
using System;
using System.Collections.Generic;

namespace FamUnion.Core.Interface
{
    public interface IReunionRepository
    {
        IEnumerable<Reunion> GetReunions();
        Reunion GetReunion(Guid id);
        Reunion SaveReunion(Reunion reunion);
    }
}
