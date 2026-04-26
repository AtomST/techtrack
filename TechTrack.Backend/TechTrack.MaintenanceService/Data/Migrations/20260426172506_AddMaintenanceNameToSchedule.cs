using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.MaintenanceService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceNameToSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "maintenance_name",
                table: "maintenance_schedule",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "maintenance_name",
                table: "maintenance_schedule");
        }
    }
}
