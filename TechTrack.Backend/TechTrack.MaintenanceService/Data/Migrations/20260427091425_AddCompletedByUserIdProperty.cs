using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.MaintenanceService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletedByUserIdProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "complited_at",
                table: "maintenance_log",
                newName: "completed_at");

            migrationBuilder.AlterColumn<Guid>(
                name: "responsible_user_id",
                table: "maintenance_log",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "completed_by_user_id",
                table: "maintenance_log",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "completed_by_user_id",
                table: "maintenance_log");

            migrationBuilder.RenameColumn(
                name: "completed_at",
                table: "maintenance_log",
                newName: "complited_at");

            migrationBuilder.AlterColumn<Guid>(
                name: "responsible_user_id",
                table: "maintenance_log",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
