using System;
using System.Collections.Generic;
using System.Text;

namespace FamUnion.Auth
{
    public class AuthConfig
    {
        public string Domain { get; set; }
        public string AuthToken { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Audience { get; set; }
    }
}
