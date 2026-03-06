using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TechTrack.OrganizationService.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEquipmentStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_equipments_equipment_status_current_status_id",
                table: "equipments");

            migrationBuilder.DropTable(
                name: "equipment_status");

            migrationBuilder.DropIndex(
                name: "IX_equipments_current_status_id",
                table: "equipments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "equipment_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment_status", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_equipments_current_status_id",
                table: "equipments",
                column: "current_status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_equipments_equipment_status_current_status_id",
                table: "equipments",
                column: "current_status_id",
                principalTable: "equipment_status",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
