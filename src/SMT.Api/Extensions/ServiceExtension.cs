﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SMT.Access.Data;
using SMT.Access.Identity;
using SMT.Access.Repository;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Repository.Statics;
using SMT.Access.Unit;
using SMT.Notification;
using SMT.Security;
using SMT.Services;
using SMT.Services.Interfaces;
using SMT.Services.Interfaces.FileSystem;
using SMT.Services.Mapping;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
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

            //services.AddMvc(/*options =>
            //                {
            //                    options.Filters.Add<LinkRewritingFilter>();
            //                }*/);



            services.AddCors(options =>
            {
                options.AddPolicy(options.DefaultPolicyName,
                                                        policy => policy.AllowAnyOrigin()
                                                                .AllowAnyHeader()
                                                                .AllowAnyMethod());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SMT.Api", Version = "v1" });
            });

            services.AddDbContext<AppDbContext>(options =>
                            options.UseSqlServer(configuration.GetConnectionString("DbConnection"),
                            s => s.UseHierarchyId()));

            services.AddDbContext<AppIdentityDbContext>(options =>
                            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<AppIdentityDbContext>()
                        .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Secret"]))
                };
            });

            //services.BuildServiceProvider().GetService<AppDbContext>().Database.Migrate();
            //services.BuildServiceProvider().GetService<AppIdentityDbContext>().Database.Migrate();


            services.AddControllers();
            services.AddAutoMapper(typeof(ModelToResourceProfile), typeof(ResourceToModelProfile));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ITelegramBotClient>(conf => new TelegramBotClient(configuration.GetValue<string>("AppSettings:BotToken")));
            services.AddScoped<INotificationService>(conf => new NotificationService(conf.GetRequiredService<ITelegramBotClient>(),
                Convert.ToInt64(configuration["AppSettings:QCChatId"]),
                Convert.ToInt64(configuration["AppSettings:ReapirChatID"])));


            /*************   Repository  ************/

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDefectRepository, DefectRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<ILineRepository, LineRepository>();
            services.AddScoped<ILineDefectRepository, LineDefectRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IMachineRepository, MachineRepository>();
            services.AddScoped<IMachineRepairRepository, MachineRepairRepository>();
            services.AddScoped<IMachineRepairerRepository, MachineRepairerRepository>();
            services.AddScoped<IStaticsRepository, StaticsRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IPlanActivityRepository, PlanActivityRepository>();

            /*************   Services  ************/

            services.AddScoped<IDefectService, DefectService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IModelService, ModelService>();
            services.AddScoped<ILineService, LineService>();
            services.AddScoped<ILineDefectService, LineDefectService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IMachineService, MachineService>();
            services.AddScoped<IMachineRepairService, MachineRepairService>();
            services.AddScoped<IMachineRepairerService, MachineRepairerService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IPlanActivityService, PlanActivityService>();


            /*************   Security  ************/
            services.AddScoped<IUserService, UserService>();


            /*************  Other  **************/
            services.AddScoped<IImageService, ImageService>();
            services.AddSingleton<IFileSystem, FileSystem>();

            //AddServices(services);

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
