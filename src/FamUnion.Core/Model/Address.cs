using System;
using System.ComponentModel.DataAnnotations;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Model
{
    public class Address : ModelBase
    {
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        public AddressEntityType AddressType { get; set; }
        [MaxLength(100)]
        public string Line1 { get; set; }
        [MaxLength(100)]
        public string Line2 { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        [MaxLength(2)]
        public string State { get; set; }
        [MaxLength(5)]
        public string ZipCode { get; set; }

        public long? Latitude { get; set; }
        public long? Longitude { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Description)
                && !string.IsNullOrEmpty(City) 
                && !string.IsNullOrEmpty(State);
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Address))
            {
                throw new ArgumentException("Object is not of the correct Type", nameof(obj));
            }

            Address updated = obj as Address;

            return Description.Trim() == updated.Description.Trim()
                && (Line1?.Trim() ?? "") == (updated.Line1?.Trim() ?? "")
                && (Line2?.Trim() ?? "") == (updated.Line2?.Trim() ?? "")
                && (City?.Trim() ?? "") == (updated.City?.Trim() ?? "")
                && (State?.Trim() ?? "") == (updated.State?.Trim() ?? "")
                && (ZipCode?.Trim() ?? "") == (updated.ZipCode?.Trim() ?? "");
        }
    }
}
