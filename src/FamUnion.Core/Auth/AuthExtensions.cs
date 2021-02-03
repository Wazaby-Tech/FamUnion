using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using FamUnion.Core.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamUnion.Auth
{
    public static class AuthExtensions
    {
        public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var authConfig = configuration.GetSection(ConfigSections.AuthKey).Get<AuthConfig>();
            services.AddSingleton(authConfig);

            services.AddSingleton<IManagementConnection, HttpClientManagementConnection>();
            services.AddSingleton<IAuthenticationConnection, HttpClientAuthenticationConnection>();

            services.AddTransient<IAuthenticateService, AuthenticateService>();
        }

        public static void ConfigureAuthWeb(this IServiceCollection services, IConfiguration configuration)
        {
            var authConfig = configuration.GetSection(ConfigSections.AuthKey).Get<AuthConfig>();
            services.AddSingleton(authConfig);
        }
    }
}
