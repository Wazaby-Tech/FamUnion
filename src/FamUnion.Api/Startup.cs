using FamUnion.Core.Interface;
using FamUnion.Infrastructure.Repository;
using FamUnion.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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
            app.UseMvc();
        }
    }
}
