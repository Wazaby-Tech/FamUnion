using FamUnion.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace FamUnion.WebAuth.Models
{
    public class HomeModel
    {
        public IEnumerable<Reunion> Reunions { get; set; } = Enumerable.Empty<Reunion>();
    }
}
