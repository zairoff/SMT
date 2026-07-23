using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json;
using SMT.Domain;
using SMT.Domain.BoardFlow;
using SMT.Domain.BoardFlow.V2;
using SMT.Domain.ReturnedProducts;
using SMT.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public DbSet<ReadyProductTransaction> ReadyProductTransactions { get; set; }
        public DbSet<ReturnedProductStore> ReturnedProductStores { get; set; }
        public DbSet<ReturnedProductRepair> ReturnedProductRepairs { get; set; }
        public DbSet<ReturnedProductUtilize> ReturnedProductUtilizes { get; set; }
        public DbSet<ReturnedProductTransaction> ReturnedProductTransactions { get; set; }
        public DbSet<ReturnedProductBufferZone> ReturnedProductBufferZones { get; set; }
        public DbSet<HourlyPlan> HourlyPlans { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<PcbInstruction> PcbInstructions { get; set; }
        public DbSet<QrReader> QrReaders { get; set; }
        public DbSet<BoardReport> BoardReports { get; set; }
        public DbSet<QrReaderV2> QrReadersV2 { get; set; }
        public DbSet<QrReaderV2Link> QrReaderV2Links { get; set; }
        public DbSet<BoardV2> BoardsV2 { get; set; }
        public DbSet<BoardMovementV2> BoardMovementsV2 { get; set; }
        public DbSet<ServiceCenterRepairer> ServiceCenterRepairers { get; set; }
        public DbSet<ServiceCenterRequest> ServiceCenterRequests { get; set; }
        public DbSet<ServiceCenterResult> ServiceCenterResults { get; set; }
        public DbSet<ServiceCenter> ServiceCenters { get; set; }
        public DbSet<ServiceCenterRequestSender> ServiceCenterRequestSenders { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>()
            .Property(c => c.PartNumber)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),        // Serialize to JSON
                v => JsonConvert.DeserializeObject<List<string>>(v)) // Deserialize to List<string>
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),          // Compare equality
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Generate hash code
                c => c.ToList()));

            // Board flow
            modelBuilder.Entity<BoardReport>()
                .HasIndex(b => b.QrCode);

            modelBuilder.Entity<BoardReport>()
                .HasIndex(b => new { b.QrReaderId, b.DateTime });

            modelBuilder.Entity<BoardReport>()
                .HasIndex(b => b.DateTime);

            modelBuilder.Entity<QrReader>()
                .HasIndex(q => q.Position);

            // Board flow V2
            modelBuilder.Entity<BoardV2>()
                .HasIndex(b => b.QrCode)
                .IsUnique();

            modelBuilder.Entity<BoardV2>()
                .HasIndex(b => new { b.LineId, b.Status });

            modelBuilder.Entity<BoardV2>()
                .HasIndex(b => new { b.CurrentQrReaderId, b.Status });

            modelBuilder.Entity<BoardV2>()
                .HasOne(b => b.Line)
                .WithMany()
                .HasForeignKey(b => b.LineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BoardV2>()
                .HasOne(b => b.Model)
                .WithMany()
                .HasForeignKey(b => b.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BoardV2>()
                .HasOne(b => b.CurrentQrReader)
                .WithMany()
                .HasForeignKey(b => b.CurrentQrReaderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QrReaderV2>()
                .HasIndex(q => new { q.LineId, q.Position });

            modelBuilder.Entity<QrReaderV2>()
                .HasOne(q => q.Line)
                .WithMany()
                .HasForeignKey(q => q.LineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QrReaderV2Link>()
                .HasIndex(l => new { l.FromReaderId, l.ToReaderId })
                .IsUnique();

            modelBuilder.Entity<QrReaderV2Link>()
                .HasIndex(l => l.ToReaderId);

            modelBuilder.Entity<QrReaderV2Link>()
                .HasOne(l => l.FromReader)
                .WithMany()
                .HasForeignKey(l => l.FromReaderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QrReaderV2Link>()
                .HasOne(l => l.ToReader)
                .WithMany()
                .HasForeignKey(l => l.ToReaderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BoardMovementV2>()
                .HasIndex(m => new { m.BoardId, m.DateTime });

            modelBuilder.Entity<BoardMovementV2>()
                .HasIndex(m => new { m.QrReaderId, m.DateTime });

            modelBuilder.Entity<BoardMovementV2>()
                .HasOne(m => m.Board)
                .WithMany()
                .HasForeignKey(m => m.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BoardMovementV2>()
                .HasOne(m => m.QrReader)
                .WithMany()
                .HasForeignKey(m => m.QrReaderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Report
            modelBuilder.Entity<Report>()
                .HasIndex(r => new { r.Barcode, r.Status }); 

            modelBuilder.Entity<Report>()
                .HasIndex(r => new { r.CreatedDate, r.Status });

            modelBuilder.Entity<Report>()
                .HasIndex(r => new { r.ModelId, r.LineId, r.DefectId, r.CreatedDate });

            modelBuilder.Entity<Report>()
                .HasIndex(r => new { r.ModelId, r.LineId, r.Status, r.CreatedDate });

            modelBuilder.Entity<Defect>()
                .HasIndex(d => d.Name);

            modelBuilder.Entity<Report>()
                .HasIndex(r => new { r.LineId, r.Status, r.CreatedDate });

            // Ready product transactions
            modelBuilder.Entity<Model>()
                .HasIndex(m => m.ProductBrandId);

            modelBuilder.Entity<ProductBrand>()
                .HasIndex(pb => pb.ProductId);

            modelBuilder.Entity<Model>()
                .HasIndex(m => m.SapCode);

            modelBuilder.Entity<ReadyProductTransaction>()
                .HasIndex(r => new { r.Date, r.Status });

            modelBuilder.Entity<ReadyProductTransaction>()
                .HasIndex(r => r.Date);
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
