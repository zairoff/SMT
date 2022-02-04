using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMT.Access.Identity;
using SMT.Api.ExceptionHandler;
using SMT.Api.Extensions;
using System;
using System.Threading.Tasks;

namespace SMT.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Seed(app);

            loggerFactory.AddFile(Configuration["AppSettings:LogFolder"]);

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SMT.Api v1"));

            app.UseRouting();
            app.UseCors();
            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task Seed(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                try
                {                 
                    var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    await IdentityDbContextSeed.SeedAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    //app.Logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
        }
    }
}
