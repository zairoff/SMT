using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMT.Access.Data;
using SMT.Access.Identity;
using SMT.Api.ExceptionHandler;
using SMT.Api.Extensions;
using System;
using System.IO;

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
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SMT.Api v1"));
            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            loggerFactory.AddFile(Configuration["AppSettings:LogFolder"]);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine($"{env.WebRootPath}/", Configuration["AppSettings:ImagesFolder"])),
                RequestPath = Configuration["AppSettings:RequestPath"]
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Seed(app, loggerFactory.CreateLogger<Startup>());
        }

        private static void Seed(IApplicationBuilder app, ILogger<Startup> logger)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var scopedProvider = scope.ServiceProvider;
            try
            {
                var appContext = scopedProvider.GetRequiredService<AppDbContext>();
                var identityContext = scopedProvider.GetRequiredService<AppIdentityDbContext>();

                appContext.Database.EnsureCreated();
                appContext.Database.Migrate();

                identityContext.Database.EnsureCreated();
                identityContext.Database.Migrate();

                var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
                IdentityDbContextSeed.SeedAsync(userManager, roleManager).Wait();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
    }
}
