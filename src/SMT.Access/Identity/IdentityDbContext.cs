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
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=oyna-users;Trusted_Connection=True;MultipleActiveResultSets=true";
            options.UseSqlServer(connectionString);
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppIdentityDbContext>
        {
            public AppIdentityDbContext CreateDbContext(string[] args)
            {
                var connectionString = "Server=(localdb)\\mssqllocaldb;Database=oyna-users;Trusted_Connection=True;MultipleActiveResultSets=true";

                var builder = new DbContextOptionsBuilder<AppIdentityDbContext>();
                builder.UseSqlServer(connectionString);
                return new AppIdentityDbContext(builder.Options);
            }
        }
    }
}
