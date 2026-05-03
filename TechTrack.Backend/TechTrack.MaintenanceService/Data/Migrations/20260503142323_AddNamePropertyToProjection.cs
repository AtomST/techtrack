using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.MaintenanceService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNamePropertyToProjection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "equipments_projection",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "equipments_projection");
        }
    }
}
