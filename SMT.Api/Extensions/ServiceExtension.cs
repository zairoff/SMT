using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMT.Access;
using SMT.Services;
using SMT.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Api.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IPcbReportService, PcbReportService>();
            return services;
        }
    }
}
