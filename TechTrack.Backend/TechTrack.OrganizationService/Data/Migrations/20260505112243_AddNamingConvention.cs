using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.OrganizationService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNamingConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_company_user_companies_company_id",
                table: "company_user");

            migrationBuilder.DropForeignKey(
                name: "FK_department_user_departments_department_id",
                table: "department_user");

            migrationBuilder.DropForeignKey(
                name: "FK_departments_companies_company_id",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "FK_equipments_departments_department_id",
                table: "equipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_equipments",
                table: "equipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_departments",
                table: "departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_department_user",
                table: "department_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_company_user",
                table: "company_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_companies",
                table: "companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProjections",
                table: "UserProjections");

            migrationBuilder.RenameTable(
                name: "UserProjections",
                newName: "user_projections");

            migrationBuilder.RenameColumn(
                name: "ResponsibleUserId",
                table: "equipments",
                newName: "responsible_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_equipments_department_id",
                table: "equipments",
                newName: "ix_equipments_department_id");

            migrationBuilder.RenameIndex(
                name: "IX_departments_company_id",
                table: "departments",
                newName: "ix_departments_company_id");

            migrationBuilder.RenameIndex(
                name: "IX_department_user_department_id",
                table: "department_user",
                newName: "ix_department_user_department_id");

            migrationBuilder.RenameIndex(
                name: "IX_company_user_company_id",
                table: "company_user",
                newName: "ix_company_user_company_id");

            migrationBuilder.RenameColumn(
                name: "CompanyHeadId",
                table: "companies",
                newName: "company_head_id");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "user_projections",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "user_projections",
                newName: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_equipments",
                table: "equipments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_departments",
                table: "departments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_department_user",
                table: "department_user",
                columns: new[] { "user_id", "department_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_company_user",
                table: "company_user",
                columns: new[] { "user_id", "company_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_companies",
                table: "companies",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_projections",
                table: "user_projections",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_company_user_companies_company_id",
                table: "company_user",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_department_user_departments_department_id",
                table: "department_user",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_departments_companies_company_id",
                table: "departments",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_equipments_departments_department_id",
                table: "equipments",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_user_companies_company_id",
                table: "company_user");

            migrationBuilder.DropForeignKey(
                name: "fk_department_user_departments_department_id",
                table: "department_user");

            migrationBuilder.DropForeignKey(
                name: "fk_departments_companies_company_id",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "fk_equipments_departments_department_id",
                table: "equipments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_equipments",
                table: "equipments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_departments",
                table: "departments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_department_user",
                table: "department_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_company_user",
                table: "company_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_companies",
                table: "companies");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_projections",
                table: "user_projections");

            migrationBuilder.RenameTable(
                name: "user_projections",
                newName: "UserProjections");

            migrationBuilder.RenameColumn(
                name: "responsible_user_id",
                table: "equipments",
                newName: "ResponsibleUserId");

            migrationBuilder.RenameIndex(
                name: "ix_equipments_department_id",
                table: "equipments",
                newName: "IX_equipments_department_id");

            migrationBuilder.RenameIndex(
                name: "ix_departments_company_id",
                table: "departments",
                newName: "IX_departments_company_id");

            migrationBuilder.RenameIndex(
                name: "ix_department_user_department_id",
                table: "department_user",
                newName: "IX_department_user_department_id");

            migrationBuilder.RenameIndex(
                name: "ix_company_user_company_id",
                table: "company_user",
                newName: "IX_company_user_company_id");

            migrationBuilder.RenameColumn(
                name: "company_head_id",
                table: "companies",
                newName: "CompanyHeadId");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "UserProjections",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "UserProjections",
                newName: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_equipments",
                table: "equipments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_departments",
                table: "departments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_department_user",
                table: "department_user",
                columns: new[] { "user_id", "department_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_company_user",
                table: "company_user",
                columns: new[] { "user_id", "company_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_companies",
                table: "companies",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProjections",
                table: "UserProjections",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_company_user_companies_company_id",
                table: "company_user",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_department_user_departments_department_id",
                table: "department_user",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_departments_companies_company_id",
                table: "departments",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_equipments_departments_department_id",
                table: "equipments",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
