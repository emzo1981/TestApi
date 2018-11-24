using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestApi.Helpers;
using TestApi.Models;
using TestApi.Services;


namespace TestApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddScoped<IApiClient, ApiClient>();
            services.AddSingleton<ICurrencyValidator, CurrencyValidator>();

            
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
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<FixerResponse, Currency > ()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Base));
            });
            app.UseHttpsRedirection();
            app.UseMvc(ConfigRoute);
        }
        private void ConfigRoute(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Values}/{action=Latest}");
        }
    }
}
