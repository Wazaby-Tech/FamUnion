﻿using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Model
{
    public class User : ModelBase
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserAuthType AuthType { get; set; } = UserAuthType.Unauthorized;

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(UserId) 
                && !string.IsNullOrWhiteSpace(Email) 
                && AuthType != UserAuthType.Unauthorized;
        }
    }
}
