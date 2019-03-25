using System;
using System.ComponentModel.DataAnnotations;

namespace FamUnion.Core.Model
{
    public class Address : ModelBase
    {        
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        [MaxLength(100)]
        public string Line1 { get; set; }
        [MaxLength(100)]
        public string Line2 { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(2)]
        public string State { get; set; }
        [MaxLength(5)]
        public string ZipCode { get; set; }
        
        public long? Latitude { get; set; }
        public long? Longitude { get; set; }

    }
}
