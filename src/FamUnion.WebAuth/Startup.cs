using FamUnion.Auth;
using FamUnion.Core.Auth;
using FamUnion.Core.Interface.Services;
using FamUnion.Core.Model;
using FamUnion.Core.Utility;
using FamUnion.WebAuth.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RestSharp.Serialization;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamUnion.WebAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure Auth0
            services.AddTransient<IAuthConfigService, AuthConfigService>();
            var appAuthConfig = Configuration.GetSection(ConfigSections.AppAuthKey).Get<AuthConfig>();
            var identityAuthConfig = Configuration.GetSection(ConfigSections.IdentityAuthKey).Get<AuthConfig>();

            var appConfig = Configuration.GetSection(ConfigSections.AppConfigKey).Get<AppConfig>();
            services.AddSingleton(appConfig);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => HostingEnvironment.IsProduction();
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add authentication services
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect("Auth0", options => {
                // Set the authority to your Auth0 domain
                options.Authority = $"https://{appAuthConfig.Domain}";

                // Configure the Auth0 Client ID and Client Secret
                options.ClientId = appAuthConfig.ClientId;
                options.ClientSecret = appAuthConfig.ClientSecret;

                // Set response type to code
                options.ResponseType = "code";

                // Configure the scope
                options.Scope.Clear();
                options.Scope.Add("openid");

                // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
                // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
                options.CallbackPath = new PathString("/callback");

                // Configure the Claims Issuer to be Auth0
                options.ClaimsIssuer = "Auth0";

                // Saves tokens to the AuthenticationProperties
                options.SaveTokens = true;

                options.Events = new OpenIdConnectEvents
                {
                    // handle the logout redirection 
                    OnRedirectToIdentityProviderForSignOut = (context) =>
                    {
                        var logoutUri = $"https://{appAuthConfig.Domain}/v2/logout?client_id={appAuthConfig.ClientId}";

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                // transform to absolute
                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                            }
                            logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async (context) =>
                    {
                        // Setup Auth0 client call
                        var authClient = new HttpClient();
                        authClient.BaseAddress = new Uri($"https://{appAuthConfig.Domain}/api/v2/");
                        var identityToken = TokenHelper.GetAuth0Token(identityAuthConfig);
                        authClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {identityToken.access_token}");

                        // Setup App API client call
                        var appClient = new HttpClient();
                        appClient.BaseAddress = new Uri($"{appConfig.ApiUrl}");
                        var appToken = TokenHelper.GetAuth0Token(appAuthConfig);
                        appClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {appToken.access_token}");

                        // Call App API to ensure user exists in database
                        // TODO: Need caching here so we're not making this db call every time
                        var identityId = context.Principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                        var appUser = JsonConvert.DeserializeObject<User>(await appClient.GetStringAsync($"users/id/{identityId}"));

                        // User is validated in Auth0 but not in the app database yet
                        if(appUser.AuthType == Constants.UserAuthType.Unauthorized)
                        {
                            // Pull full user object from Auth0
                            var authResp = JsonConvert.DeserializeObject<Auth0User>(await authClient.GetStringAsync($"users/{identityId}"));

                            // Create app user and save in the database with proper auth type
                            var newUser = new User()
                            {
                                UserId = identityId,
                                AuthType = Extensions.GetUserAuthType(identityId),
                                Email = authResp.email,
                                PhoneNumber = authResp.phone_number,
                                FirstName = authResp.given_name,
                                LastName = authResp.family_name
                            };

                            var userContent = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, ContentType.Json);
                            var postResp = await appClient.PostAsync("users", userContent);

                        }
                    }
                };
            });

            services.AddHttpClient("API", (options) =>
            {
                options.BaseAddress = new Uri(appConfig.ApiUrl);
            });

            services.AddHttpClient("AppUsers", (client) =>
            {
                client.BaseAddress = new Uri($"https://{appAuthConfig.Domain}/api/v2/");
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
