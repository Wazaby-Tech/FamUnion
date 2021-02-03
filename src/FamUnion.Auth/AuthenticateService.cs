using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using FamUnion.Core.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FamUnion.Auth
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly AuthConfig _authConfig;
        private readonly ILogger _logger;
        private readonly ManagementApiClient _managementApiClient;
        private readonly AuthenticationApiClient _authenticationApiClient;

        public AuthenticateService(AuthConfig authConfig, ILogger<AuthenticateService> logger, IManagementConnection managementConnection, IAuthenticationConnection authenticationConnection)
        {
            _authConfig = Validator.ThrowIfNull(authConfig, nameof(authConfig));
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
            _managementApiClient = new ManagementApiClient(_authConfig.AuthToken, new Uri($"https://{_authConfig.AuthDomain}/api/v2"), managementConnection);
            _authenticationApiClient = new AuthenticationApiClient(new Uri($"https://{_authConfig.AuthDomain}"), authenticationConnection);
        }

        public async Task<IList<User>> GetUserByEmailAsync(string email)
        {
            return await _managementApiClient.Users.GetUsersByEmailAsync(email);
        }

        public async Task CreateUserAsync(string email, string password)
        {
            var userReq = new UserCreateRequest
            {
                Email = email,
                Password = password,
                Connection = "Username-Password-Authentication",
                VerifyEmail = true
            };
            await _managementApiClient.Users.CreateAsync(userReq);
        }

        public async Task CreateFacebookUserAsync(string email)
        {
            var userReq = new UserCreateRequest
            {
                Email = email,
                Connection = "facebook"
            };
            await _managementApiClient.Users.CreateAsync(userReq);
        }

        public void AuthenticateUserAsync()
        {
            var authUrl = _authenticationApiClient.BuildAuthorizationUrl()
                .WithResponseType(AuthorizationResponseType.Token)
                .WithClient(_authConfig.ClientId)
                .WithAudience(_authConfig.Audience)
                .Build();
        }
    }
}
