using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.OrganizationService.Migrations
{
    /// <inheritdoc />
    public partial class UserHasManyDepartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_department_user",
                table: "department_user");

            migrationBuilder.AddPrimaryKey(
                name: "PK_department_user",
                table: "department_user",
                columns: new[] { "user_id", "department_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_department_user",
                table: "department_user");

            migrationBuilder.AddPrimaryKey(
                name: "PK_department_user",
                table: "department_user",
                column: "user_id");
        }
    }
}
