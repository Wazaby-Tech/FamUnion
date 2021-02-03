using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamUnion.Auth
{
    public static class AuthExtensions
    {
        private static string AuthKey = "Auth0";
        public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var authConfig = configuration.GetSection(AuthKey).Get<AuthConfig>();
            services.AddSingleton(authConfig);

            services.AddSingleton<IManagementConnection, HttpClientManagementConnection>();
            services.AddSingleton<IAuthenticationConnection, HttpClientAuthenticationConnection>();

            services.AddTransient<IAuthenticateService, AuthenticateService>();
        }
    }
}
