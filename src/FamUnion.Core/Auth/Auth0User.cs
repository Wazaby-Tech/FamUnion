using System;
using System.Collections.Generic;
using System.Text;

namespace FamUnion.Core.Auth
{
    public class Auth0User
    {
        public string user_id { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
    }
}
