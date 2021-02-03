using System;
using System.Collections.Generic;
using System.Text;

namespace FamUnion.Core.Auth
{
    public class Auth0Profile
    {
        public string Sub { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string UpdatedAt { get; set; }
    }
}
