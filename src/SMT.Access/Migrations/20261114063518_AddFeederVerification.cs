using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class AddFeederVerification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeederSetups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SourceFileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ActivatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeederSetups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeederSetups_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeederSetups_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LineMachines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Vendor = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineMachines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineMachines_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChangeoverSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    FeederSetupId = table.Column<int>(type: "int", nullable: false),
                    PreviousSetupId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeoverSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeoverSessions_FeederSetups_FeederSetupId",
                        column: x => x.FeederSetupId,
                        principalTable: "FeederSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeoverSessions_FeederSetups_PreviousSetupId",
                        column: x => x.PreviousSetupId,
                        principalTable: "FeederSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeoverSessions_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MachineModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineMachineId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineModules_LineMachines_LineMachineId",
                        column: x => x.LineMachineId,
                        principalTable: "LineMachines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeederSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineModuleId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SlotCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CurrentComponentId = table.Column<int>(type: "int", nullable: true),
                    CurrentReelCode = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    LoadedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LoadedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeederSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeederSlots_Components_CurrentComponentId",
                        column: x => x.CurrentComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeederSlots_MachineModules_MachineModuleId",
                        column: x => x.MachineModuleId,
                        principalTable: "MachineModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChangeoverItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeoverSessionId = table.Column<int>(type: "int", nullable: false),
                    FeederSlotId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    RequiredComponentId = table.Column<int>(type: "int", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeoverItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeoverItems_ChangeoverSessions_ChangeoverSessionId",
                        column: x => x.ChangeoverSessionId,
                        principalTable: "ChangeoverSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeoverItems_Components_RequiredComponentId",
                        column: x => x.RequiredComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeoverItems_FeederSlots_FeederSlotId",
                        column: x => x.FeederSlotId,
                        principalTable: "FeederSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeederScanLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    ScanType = table.Column<int>(type: "int", nullable: false),
                    ChangeoverSessionId = table.Column<int>(type: "int", nullable: true),
                    FeederSetupId = table.Column<int>(type: "int", nullable: true),
                    FeederSlotId = table.Column<int>(type: "int", nullable: true),
                    RawReelCode = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RawSlotCode = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ReelPartNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ExpectedComponentId = table.Column<int>(type: "int", nullable: true),
                    ScannedComponentId = table.Column<int>(type: "int", nullable: true),
                    Result = table.Column<int>(type: "int", nullable: false),
                    NgReason = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeederScanLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeederScanLogs_ChangeoverSessions_ChangeoverSessionId",
                        column: x => x.ChangeoverSessionId,
                        principalTable: "ChangeoverSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeederScanLogs_Components_ExpectedComponentId",
                        column: x => x.ExpectedComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeederScanLogs_Components_ScannedComponentId",
                        column: x => x.ScannedComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeederScanLogs_FeederSetups_FeederSetupId",
                        column: x => x.FeederSetupId,
                        principalTable: "FeederSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeederScanLogs_FeederSlots_FeederSlotId",
                        column: x => x.FeederSlotId,
                        principalTable: "FeederSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeederScanLogs_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeederSetupItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeederSetupId = table.Column<int>(type: "int", nullable: false),
                    FeederSlotId = table.Column<int>(type: "int", nullable: false),
                    ComponentId = table.Column<int>(type: "int", nullable: false),
                    ImportedCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeederSetupItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeederSetupItems_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeederSetupItems_FeederSetups_FeederSetupId",
                        column: x => x.FeederSetupId,
                        principalTable: "FeederSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeederSetupItems_FeederSlots_FeederSlotId",
                        column: x => x.FeederSlotId,
                        principalTable: "FeederSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeoverItems_ChangeoverSessionId_FeederSlotId",
                table: "ChangeoverItems",
                columns: new[] { "ChangeoverSessionId", "FeederSlotId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChangeoverItems_FeederSlotId",
                table: "ChangeoverItems",
                column: "FeederSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeoverItems_RequiredComponentId",
                table: "ChangeoverItems",
                column: "RequiredComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeoverSessions_FeederSetupId",
                table: "ChangeoverSessions",
                column: "FeederSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeoverSessions_LineId_Status",
                table: "ChangeoverSessions",
                columns: new[] { "LineId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeoverSessions_PreviousSetupId",
                table: "ChangeoverSessions",
                column: "PreviousSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_FeederScanLogs_ChangeoverSessionId",
                table: "FeederScanLogs",
                column: "ChangeoverSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeederScanLogs_ExpectedComponentId",
                table: "FeederScanLogs",
                column: "ExpectedComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_FeederScanLogs_FeederSetupId",
                table: "FeederScanLogs",
                column: "FeederSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_FeederScanLogs_FeederSlotId",
                table: "FeederScanLogs",
                column: "FeederSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_FeederScanLogs_LineId_CreatedAt",
                table: "FeederScanLogs",
                columns: new[] { "LineId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FeederScanLogs_Result_CreatedAt",
                table: "FeederScanLogs",
                columns: new[] { "Result", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FeederScanLogs_ScannedComponentId",
                table: "FeederScanLogs",
                column: "ScannedComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_FeederSetupItems_ComponentId",
                table: "FeederSetupItems",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_FeederSetupItems_FeederSetupId_FeederSlotId",
                table: "FeederSetupItems",
                columns: new[] { "FeederSetupId", "FeederSlotId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeederSetupItems_FeederSlotId",
                table: "FeederSetupItems",
                column: "FeederSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_FeederSetups_LineId_Status",
                table: "FeederSetups",
                columns: new[] { "LineId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FeederSetups_ModelId_LineId_Side_Version",
                table: "FeederSetups",
                columns: new[] { "ModelId", "LineId", "Side", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeederSlots_CurrentComponentId",
                table: "FeederSlots",
                column: "CurrentComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_FeederSlots_CurrentReelCode",
                table: "FeederSlots",
                column: "CurrentReelCode");

            migrationBuilder.CreateIndex(
                name: "IX_FeederSlots_MachineModuleId_Number",
                table: "FeederSlots",
                columns: new[] { "MachineModuleId", "Number" },
                unique: true,
                filter: "[Number] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FeederSlots_SlotCode",
                table: "FeederSlots",
                column: "SlotCode",
                unique: true,
                filter: "[SlotCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LineMachines_LineId",
                table: "LineMachines",
                column: "LineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MachineModules_LineMachineId_Name",
                table: "MachineModules",
                columns: new[] { "LineMachineId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeoverItems");

            migrationBuilder.DropTable(
                name: "FeederScanLogs");

            migrationBuilder.DropTable(
                name: "FeederSetupItems");

            migrationBuilder.DropTable(
                name: "ChangeoverSessions");

            migrationBuilder.DropTable(
                name: "FeederSlots");

            migrationBuilder.DropTable(
                name: "FeederSetups");

            migrationBuilder.DropTable(
                name: "MachineModules");

            migrationBuilder.DropTable(
                name: "LineMachines");
        }
    }
}
