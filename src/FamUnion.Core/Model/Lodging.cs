using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
