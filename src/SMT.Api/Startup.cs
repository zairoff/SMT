using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMT.Access.Identity;
using SMT.Api.ExceptionHandler;
using SMT.Api.Extensions;
using System;
using System.IO;
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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SMT.Api v1"));
            }
            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            loggerFactory.AddFile(Configuration["AppSettings:LogFolder"]);

            //Seed(app);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine($"{env.WebRootPath}/", Configuration["AppSettings:EmployeeImagesFolder"])),
                RequestPath = Configuration["AppSettings:EmployeeImagesPathString"]
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task Seed(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
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
