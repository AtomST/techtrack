using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.OrganizationService.Migrations
{
    /// <inheritdoc />
    public partial class Warning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_department_user_department_id",
                table: "department_user",
                column: "department_id");

            migrationBuilder.AddForeignKey(
                name: "FK_department_user_departments_department_id",
                table: "department_user",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_department_user_departments_department_id",
                table: "department_user");

            migrationBuilder.DropIndex(
                name: "IX_department_user_department_id",
                table: "department_user");
        }
    }
}
