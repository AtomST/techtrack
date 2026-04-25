using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.MaintenanceService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "schedule_recurrence_types",
                newName: "name_ru");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "maintenance_types",
                newName: "name_ru");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "maintenance_statuses",
                newName: "name_ru");

            migrationBuilder.AddColumn<string>(
                name: "name_en",
                table: "schedule_recurrence_types",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name_en",
                table: "maintenance_types",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name_en",
                table: "maintenance_statuses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "maintenance_schedule_record_id",
                table: "maintenance_log",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_maintenance_log_maintenance_schedule_record_id",
                table: "maintenance_log",
                column: "maintenance_schedule_record_id");

            migrationBuilder.AddForeignKey(
                name: "fk_maintenance_log_maintenance_schedule_maintenance_schedule_r",
                table: "maintenance_log",
                column: "maintenance_schedule_record_id",
                principalTable: "maintenance_schedule",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_maintenance_log_maintenance_schedule_maintenance_schedule_r",
                table: "maintenance_log");

            migrationBuilder.DropIndex(
                name: "ix_maintenance_log_maintenance_schedule_record_id",
                table: "maintenance_log");

            migrationBuilder.DropColumn(
                name: "name_en",
                table: "schedule_recurrence_types");

            migrationBuilder.DropColumn(
                name: "name_en",
                table: "maintenance_types");

            migrationBuilder.DropColumn(
                name: "name_en",
                table: "maintenance_statuses");

            migrationBuilder.DropColumn(
                name: "maintenance_schedule_record_id",
                table: "maintenance_log");

            migrationBuilder.RenameColumn(
                name: "name_ru",
                table: "schedule_recurrence_types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "name_ru",
                table: "maintenance_types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "name_ru",
                table: "maintenance_statuses",
                newName: "name");
        }
    }
}
