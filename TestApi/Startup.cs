using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Swashbuckle.AspNetCore.Swagger;
using TestApi.Helpers;
using TestApi.Models;
using TestApi.Responses;
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
            services.AddSingleton<ICurrencyCalculator, CurrencyCalculator>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Test API",
                    Description = "ASP.NET Core Web API",
                    TermsOfService = "None"
                 
                }
                );
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
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
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApiResponse, Currency > ()
                .ForMember(dest => dest.Rates, opt => opt.MapFrom(src => src.Rates.Select(p => new Rate { Name = p.Key, RateValue = p.Value }).ToList()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Base));           
            });
            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
                c.RoutePrefix = string.Empty;
            });
           

        }
       
        
    }
}
