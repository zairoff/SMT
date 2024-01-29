using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SMT.Domain;
using SMT.Domain.ReturnedProducts;
using System.Drawing;

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
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineRepair> MachineRepairs { get; set; }
        public DbSet<MachineRepairer> MachineRepairers { get; set; }
        public DbSet<PlanActivity> PlanActivities { get; set; }
        public DbSet<ReadyProduct> ReadyProducts { get; set; }
        public DbSet<ReadyProductTransaction> ReadyProductsTransactions { get; set; }
        public DbSet<ReturnedProductStore> ReturnedProductStores { get; set; }
        public DbSet<ReturnedProductRepair> ReturnedProductRepairs { get; set; }
        public DbSet<ReturnedProductUtilize> ReturnedProductUtilizes { get; set; }
        public DbSet<ReturnedProductTransaction> ReturnedProductTransactions { get; set; }
        public DbSet<ReturnedProductBufferZone> ReturnedProductBufferZones { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //        //.SetBasePath(Directory.GetCurrentDirectory())
            //        .AddJsonFile(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../SMT.Api/appsettings.json")))
            //        .Build();

            ////options.UseNpgsql("Server=localhost;Port=5432;Database=smtDB;User Id=postgres;Password=postgres;");

            //var connectionString = configuration.GetConnectionString("DbConnectionDev");

            var connectionString = "Server=192.168.0.103,53476; Database=smt04102022; User Id=sa; Password=Artel2020; Trusted_Connection=True";
            options.UseSqlServer(connectionString, s => s.UseHierarchyId());
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                //IConfigurationRoot configuration = new ConfigurationBuilder()
                //    //.SetBasePath(Directory.GetCurrentDirectory())
                //    .AddJsonFile(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../SMT.Api/appsettings.json")))
                //    .Build();
                //var connectionString = configuration.GetConnectionString("DbConnectionDev");

                var connectionString = "Server=192.168.0.103,53476; Database=smt04102022; User Id=sa; Password=Artel2020; Trusted_Connection=True";

                var builder = new DbContextOptionsBuilder<AppDbContext>();
                builder.UseSqlServer(connectionString, s => s.UseHierarchyId());
                return new AppDbContext(builder.Options);
            }
        }
    }
}
