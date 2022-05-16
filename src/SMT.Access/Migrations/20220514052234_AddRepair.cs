using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class AddRepair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Repairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repairs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Repairs_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_EmployeeId",
                table: "Repairs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_ReportId",
                table: "Repairs",
                column: "ReportId");

            migrationBuilder.Sql(@"
                CREATE TRIGGER reports_update_Trigger ON DBO.Repairs
                AFTER INSERT
                AS
                Begin
	                update r set r.status = 1 from reports r
                    join inserted i on r.id = i.reportId
                    and r.id = i.reportId;
                End
                ");
            
            migrationBuilder.Sql(@"
                CREATE TRIGGER reports_update_back_Trigger ON DBO.Repairs
                AFTER DELETE
                AS
                Begin
	                update r set r.status = 0 from reports r
                    join deleted d on r.id = d.reportId
                    and r.id = d.reportId;
                End
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Repairs");

            migrationBuilder.Sql(@"
                Drop TRIGGER reports_update_Trigger
                ");
            
            migrationBuilder.Sql(@"
                Drop TRIGGER reports_update_back_Trigger
                ");
        }
    }
}
