using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SMT.Access.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
                                : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //        //.SetBasePath(Directory.GetCurrentDirectory())
            //        .AddJsonFile(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../SMT.Api/appsettings.json")))
            //        .Build();

            ////options.UseNpgsql("Server=localhost;Port=5432;Database=smtDB;User Id=postgres;Password=postgres;");

            //var connectionString = configuration.GetConnectionString("IdentityConnection");
            //options.UseNpgsql(connectionString);

            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=smtDB;Trusted_Connection=True;MultipleActiveResultSets=true";
            options.UseSqlServer(connectionString);
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppIdentityDbContext>
        {
            public AppIdentityDbContext CreateDbContext(string[] args)
            {
                //IConfigurationRoot configuration = new ConfigurationBuilder()
                //    //.SetBasePath(Directory.GetCurrentDirectory())
                //    .AddJsonFile(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../SMT.Api/appsettings.json")))
                //    .Build();
                //var connectionString = configuration.GetConnectionString("IdentityConnection");

                var connectionString = "Server=(localdb)\\mssqllocaldb;Database=smtDB;Trusted_Connection=True;MultipleActiveResultSets=true";

                var builder = new DbContextOptionsBuilder<AppIdentityDbContext>();
                builder.UseSqlServer(connectionString);
                return new AppIdentityDbContext(builder.Options);
            }
        }
    }
}
