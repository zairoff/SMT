using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using SMT.Access.Repository.BoardFlowV2;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Repository.Interfaces.ReturnedProducts;
using SMT.Access.Repository.Interfaces.Service;
using SMT.Access.Repository.ReturnedProducts;
using SMT.Access.Repository.Service;
using SMT.Access.Repository.Statics;
using SMT.Access.Unit;
using SMT.Notification;
using SMT.Security;
using SMT.Services;
using SMT.Services.BoardFlow;
using SMT.Services.BoardFlowV2;
using SMT.Services.GoogleSheets;
using SMT.Services.Interfaces;
using SMT.Services.Interfaces.BoardFlowV2;
using SMT.Services.Interfaces.FileSystem;
using SMT.Services.Interfaces.ReturnedProducts;
using SMT.Services.Mapping;
using SMT.Services.ReturnedProducts;
using SMT.Services.Service;
using System;
using System.Linq;
using System.Net.Http;
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
                            options.UseSqlServer(configuration.GetConnectionString("DbConnection"), builder =>
                            {
                                builder.UseHierarchyId();
                            }                          
                        ));

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
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<ITelegramBotClient>(conf => new TelegramBotClient(configuration.GetValue<string>("AppSettings:BotToken")));
            services.AddTransient<INotificationService>(conf => new NotificationService(conf.GetRequiredService<ITelegramBotClient>(),
                Convert.ToInt64(configuration["AppSettings:QCChatId"]),
                Convert.ToInt64(configuration["AppSettings:ReapirChatID"]),
                Convert.ToInt64(configuration["AppSettings:ReadyProductsChatId"])));


            /*************   Repository  ************/

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<IDefectRepository, DefectRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IModelRepository, ModelRepository>();
            services.AddTransient<IPcbReportRepository, PcbReportRepository>();
            services.AddTransient<IProductBrandRepository, ProductBrandRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ILineRepository, LineRepository>();
            services.AddTransient<ILineDefectRepository, LineDefectRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IRepairAuditRepository, RepairAuditRepository>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IPcbRepairerRepository, PcbRepairerRepository>();
            services.AddTransient<IMachineRepository, MachineRepository>();
            services.AddTransient<IMachineRepairRepository, MachineRepairRepository>();
            services.AddTransient<IMachineRepairerRepository, MachineRepairerRepository>();
            services.AddTransient<IStaticsRepository, StaticsRepository>();
            services.AddTransient<IPlanRepository, PlanRepository>();
            services.AddTransient<IPlanActivityRepository, PlanActivityRepository>();
            services.AddTransient<IReadyProductRepository, ReadyProductRepository>();
            services.AddTransient<IReadyProductTransactionRepository, ReadyProductTransactionRepository>();

            services.AddTransient<IReturnedProductRepairRepository, ReturnedProductRepairRepository>();
            services.AddTransient<IReturnedProductStoreRepository, ReturnedProductStoreRepository>();
            services.AddTransient<IReturnedProductUtilizeRepository, ReturnedProductUtilizeRepository>();
            services.AddTransient<IReturnedProductTransactionRepository, ReturnedProductTransactionRepository>();
            services.AddTransient<IReturnedProductBufferRepository, ReturnedProductBufferRepository>();

            services.AddTransient<IHourlyPlanRepository, HourlyPlanRepository>();
            services.AddTransient<IComponentRepository, ComponentRepository>();
            services.AddTransient<IPcbInstructionRepository, PcbInstructionRepository>();
            services.AddTransient<IQrReaderRepository, QrReaderRepository>();
            services.AddTransient<IBoardReportRepository, BoardReportRepository>();
            services.AddTransient<IQrReaderV2Repository, QrReaderV2Repository>();
            services.AddTransient<IQrReaderV2LinkRepository, QrReaderV2LinkRepository>();
            services.AddTransient<IBoardV2Repository, BoardV2Repository>();
            services.AddTransient<IBoardMovementV2Repository, BoardMovementV2Repository>();

            services.AddTransient<IServiceCenterRepository, ServiceCenterRepository>();
            services.AddTransient<IServiceCenterRepairerRepository, ServiceCenterRepairerRepository>();
            services.AddTransient<IServiceCenterRequestRepository, ServiceCenterRequestRepository>();
            services.AddTransient<IServiceCenterResultRepository, ServiceCenterResultRepository>();
            services.AddTransient<IRequestSenderRepository, RequestSenderRepository>();

            services.AddTransient<IInstructionPositionRepository, InstructionPositionRepository>();
            services.AddTransient<ILineActiveModelRepository, LineActiveModelRepository>();
            services.AddTransient<IModelInstructionImageRepository, ModelInstructionImageRepository>();

            /*************   Services  ************/

            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IDefectService, DefectService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IModelService, ModelService>();
            services.AddTransient<IPcbReportService, PcbReportService>();
            services.AddTransient<IProductBrandService, ProductBrandService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ILineService, LineService>();
            services.AddTransient<ILineDefectService, LineDefectService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IRepairAuditService, RepairAuditService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IPcbRepairerService, PcbRepairerService>();
            services.AddTransient<IMachineService, MachineService>();
            services.AddTransient<IMachineRepairService, MachineRepairService>();
            services.AddTransient<IMachineRepairerService, MachineRepairerService>();
            services.AddTransient<IPlanService, PlanService>();
            services.AddTransient<IPlanActivityService, PlanActivityService>();
            services.AddTransient<IReadyProductService, ReadyProductService>();
            services.AddTransient<IReadyProductTransactionService, ReadyProductTransactionService>();
            services.AddTransient<IReturnedProductTransactionService, ReturnedProductTransactionService>();
            services.AddTransient<IHourlyPlanService, HourlyPlanService>();
            services.AddTransient<IComponentService, ComponentService>();
            services.AddHttpClient();
            services.AddTransient<IGoogleSheetsComponentImportService>(conf =>
                new GoogleSheetsComponentImportService(
                    conf.GetRequiredService<IHttpClientFactory>().CreateClient(),
                    conf.GetRequiredService<IComponentService>(),
                    configuration["AppSettings:ComponentImportSpreadsheetId"]));
            services.AddTransient<IPrinterService, PrinterService>();
            services.AddTransient<IPcbInstructionService, PcbInstructionService>();
            services.AddTransient<IPcbInstructionService, PcbInstructionService>();
            services.AddTransient<IQrReaderService, QrReaderService>();
            services.AddTransient<IBoardReportService, BoardReportService>();
            services.AddTransient<IQrReaderV2Service, QrReaderV2Service>();
            services.AddTransient<IBoardV2Service, BoardV2Service>();

            services.AddTransient<IServiceCenterService, ServiceCenterService>();
            services.AddTransient<IServiceCenterRepairerService, ServiceCenterRepairerService>();
            services.AddTransient<IServiceCenterRequestService, ServiceCenterRequestService>();
            services.AddTransient<IServiceCenterResultService, ServiceCenterResultService>();
            services.AddTransient<IRequestSenderService, RequestSenderService>();

            services.AddTransient<IInstructionPositionService, InstructionPositionService>();
            services.AddTransient<ILineActiveModelService, LineActiveModelService>();
            services.AddTransient<IModelInstructionImageService, ModelInstructionImageService>();

            /*************   Security  ************/
            services.AddTransient<IUserService, UserService>();


            /*************  Other  **************/
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IFileSystem, FileSystem>();

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
