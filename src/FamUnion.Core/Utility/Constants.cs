using System.Collections.Generic;

namespace FamUnion.Core.Utility
{
    public static class Constants
    {
        public enum EntityType
        {
            Reunion = 1,
            Event = 2,
            Family = 3,
            Lodging = 4
        }

        public enum AttendeeResponseStatus
        {
            NotSent = 0,
            NotRead = 1,
            Read = 2,
            Accepted = 3,
            Declined = 4
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
            SeeDescription = -1,
            Casual = 0,
            Informal = 1,
            SemiFormal = 2,
            Formal = 3,
            BlackTie = 4
        }

        public enum OrganizerAction
        {
            List = 1,
            Add = 2,
            Remove = 3,
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
