using FamUnion.Core.Interface;
using FamUnion.Infrastructure.Repository;
using FamUnion.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FamUnion.Api
{
    public class Startup
    {
		private readonly string DbKey = "Data:FamUnionDb";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // DB Connection
            var dbConnection = Configuration.GetValue<string>(DbKey);

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
            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

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
