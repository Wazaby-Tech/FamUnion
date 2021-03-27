using System.Collections.Generic;

namespace FamUnion.Core.Utility
{
    public static class Constants
    {
        public enum AddressEntityType
        {
            Reunion = 1,
            Event = 2,
            Family = 3,
            Lodging = 4
        }
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

        public enum UserAuthType : int
        {
            Unauthorized = 0,
            Auth0 = 1,
            Facebook = 2,
            Google = 3
        }

        public enum EventAttireType
        {
            Casual = 0,
            Informal = 1,
            SemiFormal = 2,
            Formal = 3,
            BlackTie = 4
        }
    }

    public static class ConfigSections
    {
        public static string AppAuthKey = "AppAuth";
        public static string IdentityAuthKey = "IdentityAuth";
        public static string UseAuthKey = "Auth0:Lock";
        public static string DbKey = "FamUnionDb";
        public static string AppConfigKey = "AppConfig";
    }
}
