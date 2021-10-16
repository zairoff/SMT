using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Access.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Defect> Defects { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(@Directory.GetCurrentDirectory() + "/../SMT.Api/appsettings.json")
                    .Build();
                var builder = new DbContextOptionsBuilder<AppDbContext>();
                var connectionString = configuration.GetConnectionString("DatabaseConnection");
                builder.UseSqlServer(connectionString);
                return new AppDbContext(builder.Options);
            }
        }
    }
}
