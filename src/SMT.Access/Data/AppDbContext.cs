using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SMT.Domain;
using System;

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
        public DbSet<Barcode> Barcodes { get; set; }
        public DbSet<EmployeeCareer> EmployeeCareers { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistories { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanDetail> PlanDetails { get; set; }
        public DbSet<LineDefect> LineDefects { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<PcbRepairer> PcbRepairers { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineRepair> MachineRepairs { get; set; }
        public DbSet<MachineRepairer> MachineRepairers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    //IConfigurationRoot configuration = new ConfigurationBuilder()
        //    //        //.SetBasePath(Directory.GetCurrentDirectory())
        //    //        .AddJsonFile(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../SMT.Api/appsettings.json")))
        //    //        .Build();

        //    ////options.UseNpgsql("Server=localhost;Port=5432;Database=smtDB;User Id=postgres;Password=postgres;");

        //    //var connectionString = configuration.GetConnectionString("DbConnectionDev");

        //    var connectionString = "Server=(localdb)\\mssqllocaldb;Database=smtDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        //    options.UseSqlServer(connectionString, s => s.UseHierarchyId());
        //}

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                //IConfigurationRoot configuration = new ConfigurationBuilder()
                //    //.SetBasePath(Directory.GetCurrentDirectory())
                //    .AddJsonFile(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../SMT.Api/appsettings.json")))
                //    .Build();
                //var connectionString = configuration.GetConnectionString("DbConnectionDev");

                var connectionString = "Server=(localdb)\\mssqllocaldb;Database=smtDB;Trusted_Connection=True;MultipleActiveResultSets=true";

                var builder = new DbContextOptionsBuilder<AppDbContext>();
                builder.UseSqlServer(connectionString, s => s.UseHierarchyId());
                return new AppDbContext(builder.Options);
            }
        }
    }
}
