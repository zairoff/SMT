using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class RemovedEmployeeFromReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Employees_EmployeeId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_EmployeeId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "Creaeted",
                table: "Reports",
                newName: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Reports",
                newName: "Creaeted");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId1",
                table: "Reports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Reports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_EmployeeId1",
                table: "Reports",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Employees_EmployeeId1",
                table: "Reports",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
