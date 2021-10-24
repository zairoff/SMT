using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SMT.Api.Extensions;
using SMT.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Api
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
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddAutoMapper(typeof(ModelToResourceProfile), typeof(ResourceToModelProfile));
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(Configuration["AppSettings:CORS"].ToString(), 
                    policy => policy.AllowAnyOrigin()
                                     .AllowAnyMethod()
                                     .AllowAnyHeader());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SMT.Api", Version = "v1" });
            });
            services.AddServices(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SMT.Api v1"));
            }

            app.UseCors(Configuration["AppSettings:CORS"]);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
