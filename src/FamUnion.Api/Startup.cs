using FamUnion.Core.Auth;
using FamUnion.Core.Interface;
using FamUnion.Core.Utility;
using FamUnion.Infrastructure.Repository;
using FamUnion.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

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
            var dbConnection = Configuration.GetValue<string>(ConfigSections.DbKey);

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

            // Services
            services.AddTransient<IReunionService, ReunionService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IAddressService, AddressService>();

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

            // Authentication
            if (Configuration.GetSection(ConfigSections.AuthKey).Exists())
            {
                var domain = $"https://{Configuration[$"{ConfigSections.AuthKey}:Domain"]}/";
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = Configuration[$"{ConfigSections.AuthKey}:Audience"];
                });

                services.AddAuthorizationCore(options =>
                {
                    foreach (var (PolicyName, Policy) in AppClaimPolicy.AppPolicies)
                    {
                        options.AddPolicy(PolicyName, Policy);
                    }

                    options.DefaultPolicy = AppClaimPolicy.AppPolicies.Single(ap => ap.PolicyName == AppClaimPolicy.Access).Policy;
                });
            }

            services.AddControllers();

            services.AddOpenApiDocument(); // add OpenAPI v3 document
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

            if(Configuration.GetSection(ConfigSections.AuthKey).Exists())
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            app.UseEndpoints(options =>
            {
                options.MapControllers();
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
    }
}
