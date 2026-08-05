using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTrack.NotificationService.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixNotificationNotificationTyperelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_notification_log_notification_type_id",
                table: "notification_log",
                column: "notification_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_notification_log_notification_types_notification_type_id",
                table: "notification_log",
                column: "notification_type_id",
                principalTable: "notification_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_notification_log_notification_types_notification_type_id",
                table: "notification_log");

            migrationBuilder.DropIndex(
                name: "ix_notification_log_notification_type_id",
                table: "notification_log");
        }
    }
}
