using FamUnion.Auth;
using FamUnion.Core.Auth;
using FamUnion.Core.Interface.Services;
using FamUnion.Core.Model;
using FamUnion.Core.Utility;
using FamUnion.WebAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamUnion.WebAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthConfig _appAuthConfig;
        private readonly AuthConfig _identityAuthConfig;
        private readonly HttpClient _apiClient;
        private readonly HttpClient _appUsersClient;

        public HomeController(ILogger<HomeController> logger, IAuthConfigService authConfigService, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _appAuthConfig = authConfigService.GetConfig(ConfigSections.AppAuthKey);
            _identityAuthConfig = authConfigService.GetConfig(ConfigSections.IdentityAuthKey);
            _apiClient = clientFactory.CreateClient("API");
            _appUsersClient = clientFactory.CreateClient("AppUsers");
        }

        public async Task<IActionResult> Index()
        {
            // If the user is authenticated, then this is how you can get the access_token and id_token
            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                // if you need to check the access token expiration time, use this value
                // provided on the authorization response and stored.
                // do not attempt to inspect/decode the access token
                DateTime accessTokenExpiresAt = DateTime.Parse(
                    await HttpContext.GetTokenAsync("expires_at"),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind);

                string idToken = await HttpContext.GetTokenAsync("id_token");

                var appToken = TokenHelper.GetAuth0Token(_identityAuthConfig);
                _appUsersClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {appToken.access_token}");

                var user = await _appUsersClient.GetAsync($"users/{User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value}")
                    .ConfigureAwait(continueOnCapturedContext: false);

                // Now you can use them. For more info on when and how to use the
                // access_token and id_token, see https://auth0.com/docs/tokens
                var token = TokenHelper.GetAuth0Token(_appAuthConfig);
                _apiClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token.access_token}");

                var resp = await _apiClient.GetAsync("api/reunions");
                var respContent = await resp.Content.ReadAsStringAsync();
                var reunions = JsonConvert.DeserializeObject<IEnumerable<Reunion>>(respContent);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
