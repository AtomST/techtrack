using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.MaintenanceService.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixIssueStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_issues_equipment_statuses_equipment_status_id",
                table: "issues");

            migrationBuilder.DropIndex(
                name: "ix_issues_equipment_status_id",
                table: "issues");

            migrationBuilder.DropColumn(
                name: "equipment_status_id",
                table: "issues");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "equipment_status_id",
                table: "issues",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_issues_equipment_status_id",
                table: "issues",
                column: "equipment_status_id");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_equipment_statuses_equipment_status_id",
                table: "issues",
                column: "equipment_status_id",
                principalTable: "equipment_status",
                principalColumn: "id");
        }
    }
}
