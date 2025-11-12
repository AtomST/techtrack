using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TechTrack.OrganizationService.Migrations
{
    /// <inheritdoc />
    public partial class RenameEquipmentsAddEquipment_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_equipment",
                table: "equipment");

            migrationBuilder.RenameTable(
                name: "equipment",
                newName: "equipments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_equipments",
                table: "equipments",
                column: "id");

            migrationBuilder.CreateTable(
                name: "equipment_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment_status", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_departments_company_id",
                table: "departments",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_company_user_company_id",
                table: "company_user",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipments_current_status_id",
                table: "equipments",
                column: "current_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipments_department_id",
                table: "equipments",
                column: "department_id");

            migrationBuilder.AddForeignKey(
                name: "FK_company_user_companies_company_id",
                table: "company_user",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_departments_companies_company_id",
                table: "departments",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_equipments_departments_department_id",
                table: "equipments",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_equipments_equipment_status_current_status_id",
                table: "equipments",
                column: "current_status_id",
                principalTable: "equipment_status",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_company_user_companies_company_id",
                table: "company_user");

            migrationBuilder.DropForeignKey(
                name: "FK_departments_companies_company_id",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "FK_equipments_departments_department_id",
                table: "equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_equipments_equipment_status_current_status_id",
                table: "equipments");

            migrationBuilder.DropTable(
                name: "equipment_status");

            migrationBuilder.DropIndex(
                name: "IX_departments_company_id",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "IX_company_user_company_id",
                table: "company_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_equipments",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "IX_equipments_current_status_id",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "IX_equipments_department_id",
                table: "equipments");

            migrationBuilder.RenameTable(
                name: "equipments",
                newName: "equipment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_equipment",
                table: "equipment",
                column: "id");
        }
    }
}
