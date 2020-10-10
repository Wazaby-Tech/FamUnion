using FamUnion.Core.Model;
using System;

namespace FamUnion.Core.Request
{
    public class NewReunionRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Address Location { get; set; }
    }
}
