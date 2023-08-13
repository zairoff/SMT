using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class shift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MachineRepairs");

            migrationBuilder.DropColumn(
                name: "NotificationDate",
                table: "MachineRepairs");

            migrationBuilder.RenameColumn(
                name: "DayNight",
                table: "Plans",
                newName: "Shift");

            migrationBuilder.AddColumn<string>(
                name: "Shift",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shift",
                table: "PlanActivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shift",
                table: "MachineRepairs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shift",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Shift",
                table: "PlanActivities");

            migrationBuilder.DropColumn(
                name: "Shift",
                table: "MachineRepairs");

            migrationBuilder.RenameColumn(
                name: "Shift",
                table: "Plans",
                newName: "DayNight");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MachineRepairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationDate",
                table: "MachineRepairs",
                type: "datetime2",
                nullable: true);
        }
    }
}
