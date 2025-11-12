using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.OrganizationService.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyHeadIdProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyHeadId",
                table: "companies",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyHeadId",
                table: "companies");
        }
    }
}
