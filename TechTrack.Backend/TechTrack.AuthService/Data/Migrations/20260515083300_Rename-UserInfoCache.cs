using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.AuthService.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserInfoCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_info_cache_user_credentials_user_id",
                table: "user_info_cache");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_info_cache",
                table: "user_info_cache");

            migrationBuilder.RenameTable(
                name: "user_info_cache",
                newName: "user_info_projections");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_info_projections",
                table: "user_info_projections",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_info_projections_user_credentials_user_id",
                table: "user_info_projections",
                column: "user_id",
                principalTable: "user_credentials",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_info_projections_user_credentials_user_id",
                table: "user_info_projections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_info_projections",
                table: "user_info_projections");

            migrationBuilder.RenameTable(
                name: "user_info_projections",
                newName: "user_info_cache");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_info_cache",
                table: "user_info_cache",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_info_cache_user_credentials_user_id",
                table: "user_info_cache",
                column: "user_id",
                principalTable: "user_credentials",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
