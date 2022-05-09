using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class AddRepairs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Repairs",
               columns: table => new
               {
                   Id = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   Condition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                   EmployeeId = table.Column<int>(type: "int", nullable: false),
                   EmployeeId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                   ReportId = table.Column<int>(type: "int", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Repairs", x => x.Id);
                   table.ForeignKey(
                       name: "FK_Repairs_Employees_EmployeeId1",
                       column: x => x.EmployeeId1,
                       principalTable: "Employees",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Restrict);
                   table.ForeignKey(
                       name: "FK_Repairs_Reports_ReportId",
                       column: x => x.ReportId,
                       principalTable: "Reports",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
               });

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_EmployeeId1",
                table: "Repairs",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_ReportId",
                table: "Repairs",
                column: "ReportId");

            
            migrationBuilder.Sql(@"
                CREATE TRIGGER reports_update_Trigger ON Repairs
                AFTER INSERT
                AS
                Begin
	                update r set r.status = 1
                    from reports r 
                    inner join inserted i
                    on r.id = i.ReportId
                End
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                 name: "Repairs");

            migrationBuilder.Sql("DROP TRIGGER reports_update_Trigger");
        }
    }
}
