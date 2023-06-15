using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SMT.Domain;

namespace SMT.Access.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<EmployeeCareer> EmployeeCareers { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistories { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<LineDefect> LineDefects { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineRepair> MachineRepairs { get; set; }
        public DbSet<MachineRepairer> MachineRepairers { get; set; }
        public DbSet<PlanActivity> PlanActivities { get; set; }

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

            var connectionString = "Server=192.168.0.103,53476; Database=oyna; User Id=sa; Password=Artel2020; Trusted_Connection=True";
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

                var connectionString = "Server=192.168.0.103,53476; Database=oyna; User Id=sa; Password=Artel2020; Trusted_Connection=True";

                var builder = new DbContextOptionsBuilder<AppDbContext>();
                builder.UseSqlServer(connectionString, s => s.UseHierarchyId());
                return new AppDbContext(builder.Options);
            }
        }
    }
}
