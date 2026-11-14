using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using SMT.Access.Data;
using SMT.Access.Repository;
using SMT.Access.Repository.Feeder;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Domain.Feeder;
using SMT.Services.Feeder;
using SMT.Services.Mapping;
using System;
using System.Collections.Generic;

namespace SMT.Access.Test.Feeder
{
    // AppDbContext.OnConfiguring always switches to SQL Server, which can't coexist with the
    // InMemory provider - this subclass skips it so services run against an in-memory database.
    // Department.HierarchyId is a SQL Server-only type the InMemory provider can't map.
    public class InMemoryAppDbContext : AppDbContext
    {
        public InMemoryAppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>().Ignore(d => d.HierarchyId);
        }
    }

    // A fresh database per test with two lines, one model and a small component catalog.
    public class FeederTestContext : IDisposable
    {
        public InMemoryAppDbContext Db { get; }

        public Line Line1 { get; private set; }

        public Line Line2 { get; private set; }

        public Model Model { get; private set; }

        public Component Resistor { get; private set; }

        public Component Capacitor { get; private set; }

        public Component Diode { get; private set; }

        public FeederTestContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            Db = new InMemoryAppDbContext(options);
            Seed();
        }

        public IMapper Mapper => new MapperConfiguration(cfg => cfg.AddProfile<FeederMappingProfile>()).CreateMapper();

        public LineMachineService LineMachineService()
        {
            return new LineMachineService(new LineMachineRepository(Db), new MachineModuleRepository(Db),
                new FeederSlotRepository(Db), new LineRepository(Db), Mapper, UnitOfWork());
        }

        public FeederSetupService FeederSetupService()
        {
            return new FeederSetupService(new FeederSetupRepository(Db), new LineMachineRepository(Db),
                new FeederSlotRepository(Db), new ComponentRepository(Db), new LineRepository(Db),
                new ModelRepository(Db), Mapper, UnitOfWork());
        }

        public FeederStationService FeederStationService()
        {
            return new FeederStationService(new FeederSetupRepository(Db), new FeederSlotRepository(Db),
                new FeederScanLogRepository(Db), new LineMachineRepository(Db), new LineActiveModelRepository(Db),
                new LineRepository(Db), new ComponentRepository(Db), UnitOfWork());
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        private UnitOfWork UnitOfWork()
        {
            return new UnitOfWork(Db, NullLogger<UnitOfWork>.Instance);
        }

        private void Seed()
        {
            Line1 = new Line { Name = "SMT-1" };
            Line2 = new Line { Name = "SMT-2" };

            // ModelRepository includes ProductBrand -> Product/Brand, so they must exist.
            var productBrand = new ProductBrand
            {
                Product = new Product { Name = "TV" },
                Brand = new Brand { Name = "Artel" },
            };
            Model = new Model { Name = "MB-55", ProductBrand = productBrand };

            Resistor = new Component
            {
                RCode = "R0001",
                PartNumber = new List<string> { "RC0402-10K", "ERJ2-10K" },
                Specification = "10K 0402",
                IsActive = true,
            };
            Capacitor = new Component
            {
                RCode = "C0001",
                PartNumber = new List<string> { "CL05-100N" },
                Specification = "100nF 0402",
                IsActive = true,
            };
            Diode = new Component
            {
                RCode = "D0001",
                PartNumber = new List<string> { "1N4148W" },
                Specification = "Diode SOD-123",
                IsActive = true,
            };

            Db.AddRange(Line1, Line2, Model, Resistor, Capacitor, Diode);
            Db.SaveChanges();
        }

        public LineMachine AddMachine(Line line, params (string Module, int Slots)[] modules)
        {
            var machine = new LineMachine { LineId = line.Id, Name = "NXT-III", Vendor = MachineVendor.Fuji };

            var order = 1;
            foreach (var (moduleName, slots) in modules)
            {
                var module = new MachineModule { Name = moduleName, Order = order++ };
                for (var number = 1; number <= slots; number++)
                {
                    module.Slots.Add(new FeederSlot
                    {
                        Number = number.ToString(),
                        SlotCode = FeederCodes.BuildSlotCode(line.Id, moduleName, number.ToString()),
                    });
                }
                machine.Modules.Add(module);
            }

            Db.Add(machine);
            Db.SaveChanges();

            return machine;
        }
    }
}
