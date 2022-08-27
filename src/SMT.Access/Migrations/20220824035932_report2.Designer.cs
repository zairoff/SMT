﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SMT.Access.Data;

namespace SMT.Access.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220824035932_report2")]
    partial class report2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SMT.Domain.Barcode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.ToTable("Barcodes");
                });

            modelBuilder.Entity("SMT.Domain.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("SMT.Domain.Defect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Defects");
                });

            modelBuilder.Entity("SMT.Domain.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<HierarchyId>("HierarchyId")
                        .HasColumnType("hierarchyid");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("SMT.Domain.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("SMT.Domain.EmployeeCareer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("EmployeeId1")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId1");

                    b.ToTable("EmployeeCareers");
                });

            modelBuilder.Entity("SMT.Domain.EmployeeHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("EmployeeId1")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId1");

                    b.ToTable("EmployeeHistories");
                });

            modelBuilder.Entity("SMT.Domain.Line", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("SMT.Domain.LineDefect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DefectId")
                        .HasColumnType("int");

                    b.Property<int>("LineId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DefectId");

                    b.HasIndex("LineId");

                    b.ToTable("LineDefects");
                });

            modelBuilder.Entity("SMT.Domain.Machine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("SMT.Domain.MachineRepair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Issue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MachineId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("NotificationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("MachineId");

                    b.ToTable("MachineRepairs");
                });

            modelBuilder.Entity("SMT.Domain.MachineRepairer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("MachineRepairers");
                });

            modelBuilder.Entity("SMT.Domain.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductBrandId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductBrandId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("SMT.Domain.PcbPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PcbPositions");
                });

            modelBuilder.Entity("SMT.Domain.PcbRepairer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("PcbRepairers");
                });

            modelBuilder.Entity("SMT.Domain.PcbReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DefectId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("LineId")
                        .HasColumnType("int");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<int>("PositionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DefectId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LineId");

                    b.HasIndex("ModelId");

                    b.ToTable("PcbReports");
                });

            modelBuilder.Entity("SMT.Domain.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("LineId")
                        .HasColumnType("int");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<int>("ProducedCount")
                        .HasColumnType("int");

                    b.Property<int>("RequiredCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LineId");

                    b.HasIndex("ModelId");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("SMT.Domain.PlanDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlanId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

                    b.ToTable("PlanDetails");
                });

            modelBuilder.Entity("SMT.Domain.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SMT.Domain.ProductBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductBrands");
                });

            modelBuilder.Entity("SMT.Domain.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DefectId")
                        .HasColumnType("int");

                    b.Property<int?>("LineId")
                        .HasColumnType("int");

                    b.Property<int?>("ModelId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("DefectId");

                    b.HasIndex("LineId");

                    b.HasIndex("ModelId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("SMT.Domain.Report2", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Condition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DefectId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("LineId")
                        .HasColumnType("int");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DefectId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LineId");

                    b.HasIndex("ModelId");

                    b.ToTable("Report2s");
                });

            modelBuilder.Entity("SMT.Domain.Vacation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("EmployeeId1")
                        .HasColumnType("int");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId1");

                    b.ToTable("Vacations");
                });

            modelBuilder.Entity("SMT.Domain.Barcode", b =>
                {
                    b.HasOne("SMT.Domain.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");
                });

            modelBuilder.Entity("SMT.Domain.Employee", b =>
                {
                    b.HasOne("SMT.Domain.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("SMT.Domain.EmployeeCareer", b =>
                {
                    b.HasOne("SMT.Domain.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId1");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("SMT.Domain.EmployeeHistory", b =>
                {
                    b.HasOne("SMT.Domain.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId1");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("SMT.Domain.LineDefect", b =>
                {
                    b.HasOne("SMT.Domain.Defect", "Defect")
                        .WithMany()
                        .HasForeignKey("DefectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SMT.Domain.Line", "Line")
                        .WithMany()
                        .HasForeignKey("LineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Defect");

                    b.Navigation("Line");
                });

            modelBuilder.Entity("SMT.Domain.MachineRepair", b =>
                {
                    b.HasOne("SMT.Domain.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SMT.Domain.Machine", "Machine")
                        .WithMany()
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Machine");
                });

            modelBuilder.Entity("SMT.Domain.MachineRepairer", b =>
                {
                    b.HasOne("SMT.Domain.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("SMT.Domain.Model", b =>
                {
                    b.HasOne("SMT.Domain.ProductBrand", "ProductBrand")
                        .WithMany()
                        .HasForeignKey("ProductBrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductBrand");
                });

            modelBuilder.Entity("SMT.Domain.PcbRepairer", b =>
                {
                    b.HasOne("SMT.Domain.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("SMT.Domain.PcbReport", b =>
                {
                    b.HasOne("SMT.Domain.Defect", "Defect")
                        .WithMany()
                        .HasForeignKey("DefectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SMT.Domain.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SMT.Domain.Line", "Line")
                        .WithMany()
                        .HasForeignKey("LineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SMT.Domain.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Defect");

                    b.Navigation("Employee");

                    b.Navigation("Line");

                    b.Navigation("Model");
                });

            modelBuilder.Entity("SMT.Domain.Plan", b =>
                {
                    b.HasOne("SMT.Domain.Line", "Line")
                        .WithMany()
                        .HasForeignKey("LineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SMT.Domain.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Line");

                    b.Navigation("Model");
                });

            modelBuilder.Entity("SMT.Domain.PlanDetail", b =>
                {
                    b.HasOne("SMT.Domain.Plan", "Plan")
                        .WithMany()
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("SMT.Domain.ProductBrand", b =>
                {
                    b.HasOne("SMT.Domain.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SMT.Domain.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("SMT.Domain.Report", b =>
                {
                    b.HasOne("SMT.Domain.Defect", "Defect")
                        .WithMany()
                        .HasForeignKey("DefectId");

                    b.HasOne("SMT.Domain.Line", "Line")
                        .WithMany()
                        .HasForeignKey("LineId");

                    b.HasOne("SMT.Domain.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId");

                    b.Navigation("Defect");

                    b.Navigation("Line");

                    b.Navigation("Model");
                });

            modelBuilder.Entity("SMT.Domain.Report2", b =>
                {
                    b.HasOne("SMT.Domain.Defect", "Defect")
                        .WithMany()
                        .HasForeignKey("DefectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SMT.Domain.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SMT.Domain.Line", "Line")
                        .WithMany()
                        .HasForeignKey("LineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SMT.Domain.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Defect");

                    b.Navigation("Employee");

                    b.Navigation("Line");

                    b.Navigation("Model");
                });

            modelBuilder.Entity("SMT.Domain.Vacation", b =>
                {
                    b.HasOne("SMT.Domain.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId1");

                    b.Navigation("Employee");
                });
#pragma warning restore 612, 618
        }
    }
}
