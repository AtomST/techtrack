using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.MaintenanceService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_resolved",
                table: "issues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "resolved_by_maintenance_id",
                table: "issues",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "maintenance_log",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenance_log", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_issues_resolved_by_maintenance_id",
                table: "issues",
                column: "resolved_by_maintenance_id");

            migrationBuilder.AddForeignKey(
                name: "FK_issues_maintenance_log_resolved_by_maintenance_id",
                table: "issues",
                column: "resolved_by_maintenance_id",
                principalTable: "maintenance_log",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_issues_maintenance_log_resolved_by_maintenance_id",
                table: "issues");

            migrationBuilder.DropTable(
                name: "maintenance_log");

            migrationBuilder.DropIndex(
                name: "IX_issues_resolved_by_maintenance_id",
                table: "issues");

            migrationBuilder.DropColumn(
                name: "is_resolved",
                table: "issues");

            migrationBuilder.DropColumn(
                name: "resolved_by_maintenance_id",
                table: "issues");
        }
    }
}
