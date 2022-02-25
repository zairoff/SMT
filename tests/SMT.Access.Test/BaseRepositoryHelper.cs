using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SMT.Access.Data;
using SMT.Access.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Access.Test
{
    public abstract class BaseRepositoryHelper : IDisposable
    {
        protected readonly AppDbContext _context;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseRepositoryHelper()
        {
            var dbSuffix = Guid.NewGuid();

            var serviceProvider = new ServiceCollection()
                                    .AddEntityFrameworkNpgsql()
                                    .BuildServiceProvider();

            var connection = $"Host=localhost;Port=5432;Database=integration_tests_{dbSuffix};Username=postgres;Password=postgres;Maximum Pool Size=1";

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseNpgsql(connection)
                    .UseInternalServiceProvider(serviceProvider);

            var dbOptions = builder.Options;

            _context = new AppDbContext(dbOptions);

            _unitOfWork = new UnitOfWork(_context);

            _context.Database.EnsureCreated();
        }
        public void Dispose() => _context.Database.EnsureDeleted();
    }
}
