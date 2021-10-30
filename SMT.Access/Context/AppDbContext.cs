using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<PcbPosition> PcbPositions { get; set; }
        public DbSet<PcbReport> PcbReports { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Repairer> Repairers { get; set; }
        public DbSet<Report> Reports { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(@Directory.GetCurrentDirectory() + "/../SMT.Api/appsettings.json")
                    .Build();
            var connectionString = configuration.GetConnectionString("DbConnectionDev");
            options.UseSqlServer(connectionString, conf =>
            {
                conf.UseHierarchyId();
            });
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(@Directory.GetCurrentDirectory() + "/../SMT.Api/appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("DbConnectionDev");

                var builder = new DbContextOptionsBuilder<AppDbContext>();
                builder.UseSqlServer(connectionString);
                return new AppDbContext(builder.Options);
            }
        }
    }
}
