using System.ComponentModel.DataAnnotations;

namespace FamUnion.Core.Model
{
    public class Lodging : ModelBase
    {
        [Required]  
        public string Name { get; set; }
        public string Description { get; set; }
        
        public virtual Address Location { get; set; }
    }
}
