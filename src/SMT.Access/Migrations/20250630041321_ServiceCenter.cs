using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class ServiceCenter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceCenterRepairers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCenterRepairers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCenters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCenters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCenterRequestSenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceCenterId = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCenterRequestSenders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCenterRequestSenders_ServiceCenters_ServiceCenterId",
                        column: x => x.ServiceCenterId,
                        principalTable: "ServiceCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCenterRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    ServiceCenterId = table.Column<int>(type: "int", nullable: false),
                    RequestSenderId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCenterRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCenterRequests_ServiceCenterRequestSenders_RequestSenderId",
                        column: x => x.RequestSenderId,
                        principalTable: "ServiceCenterRequestSenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCenterResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceCenterRequestId = table.Column<int>(type: "int", nullable: false),
                    ServiceCenterRepairerId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCenterResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCenterResults_ServiceCenterRepairers_ServiceCenterRepairerId",
                        column: x => x.ServiceCenterRepairerId,
                        principalTable: "ServiceCenterRepairers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceCenterResults_ServiceCenterRequests_ServiceCenterRequestId",
                        column: x => x.ServiceCenterRequestId,
                        principalTable: "ServiceCenterRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterRequests_RequestSenderId",
                table: "ServiceCenterRequests",
                column: "RequestSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterRequestSenders_ServiceCenterId",
                table: "ServiceCenterRequestSenders",
                column: "ServiceCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterResults_ServiceCenterRepairerId",
                table: "ServiceCenterResults",
                column: "ServiceCenterRepairerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterResults_ServiceCenterRequestId",
                table: "ServiceCenterResults",
                column: "ServiceCenterRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceCenterResults");

            migrationBuilder.DropTable(
                name: "ServiceCenterRepairers");

            migrationBuilder.DropTable(
                name: "ServiceCenterRequests");

            migrationBuilder.DropTable(
                name: "ServiceCenterRequestSenders");

            migrationBuilder.DropTable(
                name: "ServiceCenters");
        }
    }
}
