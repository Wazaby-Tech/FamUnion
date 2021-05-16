using FamUnion.Core.Auth;
using FamUnion.Core.Interface;
using FamUnion.Core.Interface.Repository;
using FamUnion.Core.Interface.Services;
using FamUnion.Core.Utility;
using FamUnion.Core.Validation;
using FamUnion.Infrastructure.Repository;
using FamUnion.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FamUnion.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // DB Connection
            var dbConnection = Configuration.GetConnectionString(ConfigSections.DbKey);
            ConfigureRepositories(services, dbConnection);

            // Health Checks
            services.AddHealthChecks()
                .AddSqlServer(dbConnection);

            // Services
            services.AddTransient<IReunionService, ReunionService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IInviteService, InviteService>();
            services.AddTransient<IUserAccessService, UserAccessService>();

            // Singletons
            services.AddSingleton<ReunionValidator>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .Build();
                });
            });

            // App Authentication
            var domain = $"https://{Configuration[$"{ConfigSections.AppAuthKey}:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration[$"{ConfigSections.AppAuthKey}:Audience"];
            });

            services.AddAuthorizationCore(options =>
            {
                foreach (var (PolicyName, Policy) in AppClaimPolicy.AppPolicies)
                {
                    options.AddPolicy(PolicyName, Policy);
                }

                options.DefaultPolicy = AppClaimPolicy.AppPolicies.Single(ap => ap.PolicyName == AppClaimPolicy.Access).Policy;
            });

            services.AddHttpClient("AppUsers", (client) =>
            {
                client.BaseAddress = new Uri(Configuration[$"{ConfigSections.IdentityAuthKey}:Audience"]);
            });

            services.AddControllers();

            services.AddOpenApiDocument(); // add OpenAPI v3 document
        }

        private static void ConfigureRepositories(IServiceCollection services, string dbConnection)
        {
            // Repositories
            services.AddTransient<IReunionRepository, ReunionRepository>(Provider =>
            {
                return new ReunionRepository(dbConnection);
            });

            services.AddTransient<IAddressRepository, AddressRepository>(Provider =>
            {
                return new AddressRepository(dbConnection);
            });

            services.AddTransient<IEventRepository, EventRepository>(Provider =>
            {
                return new EventRepository(dbConnection);
            });

            services.AddTransient<IUserRepository, UserRepository>(Provider =>
            {
                return new UserRepository(dbConnection);
            });

            services.AddTransient<IUserAccessRepository, UserAccessRepository>(Provider =>
            {
                return new UserAccessRepository(dbConnection);
            });

            services.AddTransient<IInviteRepository, InviteRepository>(Provider =>
            {
                return new InviteRepository(dbConnection);
            });
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
                app.UseHsts();
            }

            app.UseCors();
            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();            

            app.UseEndpoints(options =>
            {
                options.MapControllers();
                options.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    },
                    ResponseWriter = WriteResponse
                });
            });

            app.UseOpenApi(options =>
            {
                options.DocumentName = "FamUnion API";
            }); // serve OpenAPI/Swagger documents
            app.UseSwaggerUi3(options =>
            {
                options.DocumentTitle = "FamUnion API";
            }); // serve Swagger UI
        }

        private static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));

            return context.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }
}
