using StatesAndProvinces;
using System.Collections.Generic;

namespace FamUnion.Core.Utility
{
    public static class Constants
    {
        public enum Size
        {
            KIDS_XS = 0,
            KIDS_S,
            KIDS_M,
            KIDS_L,
            XS = 10,
            S,
            M,
            L,
            XL,
            XXL,
            XXXL,
            XXXXL
        };
        
        public enum Role
        {
            Attendee = 0,
            Organizer,
            SystemAdmin
        };

        public static IList<SubRegion> States => Factory.Make(CountrySelection.UnitedStates);
    }
}
