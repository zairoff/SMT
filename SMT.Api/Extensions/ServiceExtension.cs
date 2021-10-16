using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMT.Access;
using SMT.Access.Context;
using SMT.Services;
using SMT.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SMT.Api.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection")));

            AddServices(services);

            return services;
        }

        private static void AddServices(IServiceCollection services)
        {
            var assemblyNames = (from t in Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                                 where t.Name.Contains("SMT.Services")
                                 select t);

            foreach (var assemblyName in assemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);

                var types = from t in assembly.GetTypes()
                            where t.IsClass &&
                            t.Namespace == "SMT.Services" &&
                            t.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null
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
