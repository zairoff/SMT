using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SMT.Domain;
using System.IO;

namespace SMT.Access.Data
{
    public class AppDbContext : DbContext
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

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("../SMT.Api/appsettings.json")
                    .Build();

            //options.UseNpgsql("Server=localhost;Port=5432;Database=smtDB;User Id=postgres;Password=postgres;");

            var connectionString = configuration.GetConnectionString("DbConnectionDev");
            options.UseSqlServer(connectionString);
        }

        //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        //{
        //    public AppDbContext CreateDbContext(string[] args)
        //    {
        //        //IConfigurationRoot configuration = new ConfigurationBuilder()
        //        //    .SetBasePath(Directory.GetCurrentDirectory())
        //        //    .AddJsonFile("appsettings.json")//../SMT.Api/appsettings.json")
        //        //    .Build();
        //        //var connectionString = configuration.GetConnectionString("DbConnectionPostgres");

        //        var builder = new DbContextOptionsBuilder<AppDbContext>();
        //        builder.UseNpgsql("Server=localhost;Port=5432;Database=smtDB;User Id=postgres;Password=postgres;");
        //        return new AppDbContext(builder.Options);
        //    }
        //}
    }
}
