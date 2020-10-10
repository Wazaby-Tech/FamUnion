using FamUnion.Core.Interface;
using FamUnion.Infrastructure.Repository;
using FamUnion.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            // Repositories
            services.AddTransient<IReunionRepository, ReunionRepository>(Provider =>
            {
                return new ReunionRepository(Configuration.GetValue<string>(DbKey));
            });

            services.AddTransient<IAddressRepository, AddressRepository>(Provider =>
            {
                return new AddressRepository(Configuration.GetValue<string>(DbKey));
            });

            services.AddTransient<IEventRepository, EventRepository>(Provider =>
            {
                return new EventRepository(Configuration.GetValue<string>(DbKey));
            });

            // Services
            services.AddTransient<IReunionService, ReunionService>();
            services.AddTransient<IEventService, EventService>();

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
