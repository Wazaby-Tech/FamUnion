using FamUnion.Auth;
using FamUnion.Core.Interface.Services;
using Microsoft.Extensions.Configuration;

namespace FamUnion.WebAuth.Services
{
    public class AuthConfigService : IAuthConfigService
    {
        private readonly IConfiguration _configuration;

        public AuthConfigService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public AuthConfig GetConfig(string configKey)
        {
            return _configuration.GetSection(configKey).Get<AuthConfig>();
        }
    }
}
