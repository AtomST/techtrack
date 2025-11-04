using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.AuthService.Data.Migrations
{
    /// <inheritdoc />
    public partial class adduserinfocache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_info_cache",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: true),
                    role_name = table.Column<string>(type: "text", nullable: false, defaultValue: "Undefined")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_info_cache", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_info_cache_user_credentials_user_id",
                        column: x => x.user_id,
                        principalTable: "user_credentials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_info_cache");
        }
    }
}
