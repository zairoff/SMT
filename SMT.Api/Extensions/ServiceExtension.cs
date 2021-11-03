using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SMT.Access;
using SMT.Access.Context;
using SMT.Common.Mapping;
using SMT.Domain;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SMT.Api.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            //{

            //}
            //else
            //{
            //    services.AddDbContext<AppDbContext>(options =>
            //                options.UseSqlServer(configuration.GetConnectionString("DbConnectionDev")));
            //}

            services.AddAutoMapper(typeof(ModelToResourceProfile), typeof(ResourceToModelProfile));
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(configuration["AppSettings:CORS"].ToString(),
                    policy => policy.AllowAnyOrigin()
                                     .AllowAnyMethod()
                                     .AllowAnyHeader());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SMT.Api", Version = "v1" });
            });

            services.AddDbContext<AppDbContext>(options =>
                            options.UseSqlServer(configuration.GetConnectionString("DbConnectionDev"), o => o.UseHierarchyId()));

            services.AddIdentity<User, IdentityRole>()
                        .AddEntityFrameworkStores<AppDbContext>()
                        .AddDefaultTokenProviders();

            //services.BuildServiceProvider().GetService<AppDbContext>().Database.Migrate();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));            

            AddServices(services);

            return services;
        }

        private static void AddServices(IServiceCollection services)
        {
            var assemblyNames = (from t in Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                                 where t.Name.Contains("SMT.Services") || t.Name.Contains("SMT.Security")
                                 select t);

            foreach (var assemblyName in assemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);

                var types = from t in assembly.GetTypes()
                            where t.IsClass &&                            
                            t.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null &&
                            (t.Namespace == "SMT.Services" || t.Namespace == "SMT.Security")
                            select t;
                foreach (var type in types)
                {
                    var serviceInterface = type.GetTypeInfo().GetInterfaces().First();
                    services.AddScoped(serviceInterface, type);
                }
            }
        }
    }
}
