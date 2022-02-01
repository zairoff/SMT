using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SMT.Access;
using SMT.Access.Data;
using SMT.Access.Identity;
using SMT.Common.Mapping;
using SMT.Domain;
using SMT.Notification;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Telegram.Bot;

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

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SMT.Api", Version = "v1" });
            });

            services.AddDbContext<AppDbContext>(options =>
                            options.UseNpgsql(configuration.GetConnectionString("DbConnectionDev")));

            services.AddDbContext<AppIdentityDbContext>(options =>
                            options.UseNpgsql(configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<AppIdentityDbContext>()
                        .AddDefaultTokenProviders();

            services.BuildServiceProvider().GetService<AppDbContext>().Database.Migrate();
            services.BuildServiceProvider().GetService<AppIdentityDbContext>().Database.Migrate();
            services.AddControllers();
            services.AddAutoMapper(typeof(ModelToResourceProfile), typeof(ResourceToModelProfile));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITelegramBotClient>(conf => new TelegramBotClient(configuration.GetValue<string>("AppSettings:BotToken")));
            services.AddScoped<INotificationService>(conf => new NotificationService(conf.GetRequiredService<ITelegramBotClient>(), Convert.ToInt64(configuration["AppSettings:TelegramChatId"])));

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
                    var serviceInterfaces = type.GetTypeInfo().GetInterfaces();

                    if (serviceInterfaces.Length == 0)
                        continue;

                    services.AddScoped(serviceInterfaces.First(), type);
                }
            }
        }
    }
}
