using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TechTrack.MaintenanceService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduledMaintenance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_issues_equipment_status_EquipmentStatusId",
                table: "issues");

            migrationBuilder.DropForeignKey(
                name: "FK_issues_equipment_status_status_id",
                table: "issues");

            migrationBuilder.DropForeignKey(
                name: "FK_issues_maintenance_log_resolved_by_maintenance_id",
                table: "issues");

            migrationBuilder.DropForeignKey(
                name: "FK_OutboxMessage_InboxState_InboxMessageId_InboxConsumerId",
                table: "OutboxMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_OutboxMessage_OutboxState_OutboxId",
                table: "OutboxMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_maintenance_log",
                table: "maintenance_log");

            migrationBuilder.DropPrimaryKey(
                name: "PK_issues",
                table: "issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_equipments_projection",
                table: "equipments_projection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_equipment_status",
                table: "equipment_status");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxState",
                table: "OutboxState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessage",
                table: "OutboxMessage");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_InboxState_MessageId_ConsumerId",
                table: "InboxState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InboxState",
                table: "InboxState");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "maintenance_log",
                newName: "complited_at");

            migrationBuilder.AlterColumn<DateTime>(
                name: "complited_at",
                table: "maintenance_log",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.RenameTable(
                name: "OutboxState",
                newName: "outbox_state");

            migrationBuilder.RenameTable(
                name: "OutboxMessage",
                newName: "outbox_message");

            migrationBuilder.RenameTable(
                name: "InboxState",
                newName: "inbox_state");

            migrationBuilder.RenameColumn(
                name: "creator_id",
                table: "maintenance_log",
                newName: "responsible_user_id");

            migrationBuilder.RenameColumn(
                name: "EquipmentStatusId",
                table: "issues",
                newName: "equipment_status_id");

            migrationBuilder.RenameIndex(
                name: "IX_issues_status_id",
                table: "issues",
                newName: "ix_issues_status_id");

            migrationBuilder.RenameIndex(
                name: "IX_issues_resolved_by_maintenance_id",
                table: "issues",
                newName: "ix_issues_resolved_by_maintenance_id");

            migrationBuilder.RenameIndex(
                name: "IX_issues_EquipmentStatusId",
                table: "issues",
                newName: "ix_issues_equipment_status_id");

            migrationBuilder.RenameColumn(
                name: "Delivered",
                table: "outbox_state",
                newName: "delivered");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "outbox_state",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "outbox_state",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "LockId",
                table: "outbox_state",
                newName: "lock_id");

            migrationBuilder.RenameColumn(
                name: "LastSequenceNumber",
                table: "outbox_state",
                newName: "last_sequence_number");

            migrationBuilder.RenameColumn(
                name: "OutboxId",
                table: "outbox_state",
                newName: "outbox_id");

            migrationBuilder.RenameIndex(
                name: "IX_OutboxState_Created",
                table: "outbox_state",
                newName: "ix_outbox_state_created");

            migrationBuilder.RenameColumn(
                name: "Properties",
                table: "outbox_message",
                newName: "properties");

            migrationBuilder.RenameColumn(
                name: "Headers",
                table: "outbox_message",
                newName: "headers");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "outbox_message",
                newName: "body");

            migrationBuilder.RenameColumn(
                name: "SourceAddress",
                table: "outbox_message",
                newName: "source_address");

            migrationBuilder.RenameColumn(
                name: "SentTime",
                table: "outbox_message",
                newName: "sent_time");

            migrationBuilder.RenameColumn(
                name: "ResponseAddress",
                table: "outbox_message",
                newName: "response_address");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "outbox_message",
                newName: "request_id");

            migrationBuilder.RenameColumn(
                name: "OutboxId",
                table: "outbox_message",
                newName: "outbox_id");

            migrationBuilder.RenameColumn(
                name: "MessageType",
                table: "outbox_message",
                newName: "message_type");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "outbox_message",
                newName: "message_id");

            migrationBuilder.RenameColumn(
                name: "InitiatorId",
                table: "outbox_message",
                newName: "initiator_id");

            migrationBuilder.RenameColumn(
                name: "InboxMessageId",
                table: "outbox_message",
                newName: "inbox_message_id");

            migrationBuilder.RenameColumn(
                name: "InboxConsumerId",
                table: "outbox_message",
                newName: "inbox_consumer_id");

            migrationBuilder.RenameColumn(
                name: "FaultAddress",
                table: "outbox_message",
                newName: "fault_address");

            migrationBuilder.RenameColumn(
                name: "ExpirationTime",
                table: "outbox_message",
                newName: "expiration_time");

            migrationBuilder.RenameColumn(
                name: "EnqueueTime",
                table: "outbox_message",
                newName: "enqueue_time");

            migrationBuilder.RenameColumn(
                name: "DestinationAddress",
                table: "outbox_message",
                newName: "destination_address");

            migrationBuilder.RenameColumn(
                name: "CorrelationId",
                table: "outbox_message",
                newName: "correlation_id");

            migrationBuilder.RenameColumn(
                name: "ConversationId",
                table: "outbox_message",
                newName: "conversation_id");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "outbox_message",
                newName: "content_type");

            migrationBuilder.RenameColumn(
                name: "SequenceNumber",
                table: "outbox_message",
                newName: "sequence_number");

            migrationBuilder.RenameIndex(
                name: "IX_OutboxMessage_OutboxId_SequenceNumber",
                table: "outbox_message",
                newName: "ix_outbox_message_outbox_id_sequence_number");

            migrationBuilder.RenameIndex(
                name: "IX_OutboxMessage_InboxMessageId_InboxConsumerId_SequenceNumber",
                table: "outbox_message",
                newName: "ix_outbox_message_inbox_message_id_inbox_consumer_id_sequence_");

            migrationBuilder.RenameIndex(
                name: "IX_OutboxMessage_ExpirationTime",
                table: "outbox_message",
                newName: "ix_outbox_message_expiration_time");

            migrationBuilder.RenameIndex(
                name: "IX_OutboxMessage_EnqueueTime",
                table: "outbox_message",
                newName: "ix_outbox_message_enqueue_time");

            migrationBuilder.RenameColumn(
                name: "Received",
                table: "inbox_state",
                newName: "received");

            migrationBuilder.RenameColumn(
                name: "Delivered",
                table: "inbox_state",
                newName: "delivered");

            migrationBuilder.RenameColumn(
                name: "Consumed",
                table: "inbox_state",
                newName: "consumed");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "inbox_state",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "inbox_state",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "ReceiveCount",
                table: "inbox_state",
                newName: "receive_count");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "inbox_state",
                newName: "message_id");

            migrationBuilder.RenameColumn(
                name: "LockId",
                table: "inbox_state",
                newName: "lock_id");

            migrationBuilder.RenameColumn(
                name: "LastSequenceNumber",
                table: "inbox_state",
                newName: "last_sequence_number");

            migrationBuilder.RenameColumn(
                name: "ExpirationTime",
                table: "inbox_state",
                newName: "expiration_time");

            migrationBuilder.RenameColumn(
                name: "ConsumerId",
                table: "inbox_state",
                newName: "consumer_id");

            migrationBuilder.RenameIndex(
                name: "IX_InboxState_Delivered",
                table: "inbox_state",
                newName: "ix_inbox_state_delivered");

            migrationBuilder.AddColumn<int>(
                name: "maintenance_status_id",
                table: "maintenance_log",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "maintenance_type_id",
                table: "maintenance_log",
                type: "integer",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<Guid>(
                name: "schedule_record_id",
                table: "maintenance_log",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "scheduled_date",
                table: "maintenance_log",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_maintenance_log",
                table: "maintenance_log",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_issues",
                table: "issues",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_equipments_projection",
                table: "equipments_projection",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_equipment_status",
                table: "equipment_status",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_outbox_state",
                table: "outbox_state",
                column: "outbox_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_outbox_message",
                table: "outbox_message",
                column: "sequence_number");

            migrationBuilder.AddUniqueConstraint(
                name: "ak_inbox_state_message_id_consumer_id",
                table: "inbox_state",
                columns: new[] { "message_id", "consumer_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_inbox_state",
                table: "inbox_state",
                column: "id");

            migrationBuilder.CreateTable(
                name: "maintenance_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_maintenance_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "maintenance_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_maintenance_types", x => x.id);
                });
            migrationBuilder.InsertData(
                table: "maintenance_types",
                columns: new[] { "id", "name" },
                values: new object[,] {
                    { 1, "Плановое" },
                    { 2, "Внеплановое" },
                    { 3, "Профилактика" },
                    { 4, "Модернизация" }
                });
            migrationBuilder.CreateTable(
                name: "schedule_recurrence_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    in_days_value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_schedule_recurrence_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "maintenance_schedule",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    recurrence_type_id = table.Column<int>(type: "integer", nullable: false),
                    interval_value = table.Column<int>(type: "integer", nullable: false),
                    next_maintenance_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    responsible_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_advance_days = table.Column<int>(type: "integer", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_maintenance_schedule", x => x.id);
                    table.ForeignKey(
                        name: "fk_maintenance_schedule_schedule_recurrence_types_recurrence_t",
                        column: x => x.recurrence_type_id,
                        principalTable: "schedule_recurrence_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.InsertData(
                table: "maintenance_statuses",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "Completed" });

            migrationBuilder.CreateIndex(
                name: "ix_maintenance_log_maintenance_status_id",
                table: "maintenance_log",
                column: "maintenance_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_maintenance_log_maintenance_type_id",
                table: "maintenance_log",
                column: "maintenance_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_maintenance_schedule_recurrence_type_id",
                table: "maintenance_schedule",
                column: "recurrence_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_equipment_statuses_equipment_status_id",
                table: "issues",
                column: "equipment_status_id",
                principalTable: "equipment_status",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_equipment_statuses_status_id",
                table: "issues",
                column: "status_id",
                principalTable: "equipment_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_issues_maintenance_log_resolved_by_maintenance_id",
                table: "issues",
                column: "resolved_by_maintenance_id",
                principalTable: "maintenance_log",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_maintenance_log_maintenance_statuses_maintenance_status_id",
                table: "maintenance_log",
                column: "maintenance_status_id",
                principalTable: "maintenance_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_maintenance_log_maintenance_types_maintenance_type_id",
                table: "maintenance_log",
                column: "maintenance_type_id",
                principalTable: "maintenance_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_outbox_message_inbox_state_inbox_message_id_inbox_consumer_",
                table: "outbox_message",
                columns: new[] { "inbox_message_id", "inbox_consumer_id" },
                principalTable: "inbox_state",
                principalColumns: new[] { "message_id", "consumer_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_outbox_message_outbox_state_outbox_id",
                table: "outbox_message",
                column: "outbox_id",
                principalTable: "outbox_state",
                principalColumn: "outbox_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_issues_equipment_statuses_equipment_status_id",
                table: "issues");

            migrationBuilder.DropForeignKey(
                name: "fk_issues_equipment_statuses_status_id",
                table: "issues");

            migrationBuilder.DropForeignKey(
                name: "fk_issues_maintenance_log_resolved_by_maintenance_id",
                table: "issues");

            migrationBuilder.DropForeignKey(
                name: "fk_maintenance_log_maintenance_statuses_maintenance_status_id",
                table: "maintenance_log");

            migrationBuilder.DropForeignKey(
                name: "fk_maintenance_log_maintenance_types_maintenance_type_id",
                table: "maintenance_log");

            migrationBuilder.DropForeignKey(
                name: "fk_outbox_message_inbox_state_inbox_message_id_inbox_consumer_",
                table: "outbox_message");

            migrationBuilder.DropForeignKey(
                name: "fk_outbox_message_outbox_state_outbox_id",
                table: "outbox_message");

            migrationBuilder.DropTable(
                name: "maintenance_schedule");

            migrationBuilder.DropTable(
                name: "maintenance_statuses");

            migrationBuilder.DropTable(
                name: "maintenance_types");

            migrationBuilder.DropTable(
                name: "schedule_recurrence_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_maintenance_log",
                table: "maintenance_log");

            migrationBuilder.DropIndex(
                name: "ix_maintenance_log_maintenance_status_id",
                table: "maintenance_log");

            migrationBuilder.DropIndex(
                name: "ix_maintenance_log_maintenance_type_id",
                table: "maintenance_log");

            migrationBuilder.DropPrimaryKey(
                name: "pk_issues",
                table: "issues");

            migrationBuilder.DropPrimaryKey(
                name: "pk_equipments_projection",
                table: "equipments_projection");

            migrationBuilder.DropPrimaryKey(
                name: "pk_equipment_status",
                table: "equipment_status");

            migrationBuilder.DropPrimaryKey(
                name: "pk_outbox_state",
                table: "outbox_state");

            migrationBuilder.DropPrimaryKey(
                name: "pk_outbox_message",
                table: "outbox_message");

            migrationBuilder.DropUniqueConstraint(
                name: "ak_inbox_state_message_id_consumer_id",
                table: "inbox_state");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inbox_state",
                table: "inbox_state");

            migrationBuilder.DropColumn(
                name: "complited_at",
                table: "maintenance_log");

            migrationBuilder.DropColumn(
                name: "maintenance_status_id",
                table: "maintenance_log");

            migrationBuilder.DropColumn(
                name: "maintenance_type_id",
                table: "maintenance_log");

            migrationBuilder.DropColumn(
                name: "schedule_record_id",
                table: "maintenance_log");

            migrationBuilder.DropColumn(
                name: "scheduled_date",
                table: "maintenance_log");

            migrationBuilder.RenameTable(
                name: "outbox_state",
                newName: "OutboxState");

            migrationBuilder.RenameTable(
                name: "outbox_message",
                newName: "OutboxMessage");

            migrationBuilder.RenameTable(
                name: "inbox_state",
                newName: "InboxState");

            migrationBuilder.RenameColumn(
                name: "responsible_user_id",
                table: "maintenance_log",
                newName: "creator_id");

            migrationBuilder.RenameColumn(
                name: "equipment_status_id",
                table: "issues",
                newName: "EquipmentStatusId");

            migrationBuilder.RenameIndex(
                name: "ix_issues_status_id",
                table: "issues",
                newName: "IX_issues_status_id");

            migrationBuilder.RenameIndex(
                name: "ix_issues_resolved_by_maintenance_id",
                table: "issues",
                newName: "IX_issues_resolved_by_maintenance_id");

            migrationBuilder.RenameIndex(
                name: "ix_issues_equipment_status_id",
                table: "issues",
                newName: "IX_issues_EquipmentStatusId");

            migrationBuilder.RenameColumn(
                name: "delivered",
                table: "OutboxState",
                newName: "Delivered");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "OutboxState",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "OutboxState",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "lock_id",
                table: "OutboxState",
                newName: "LockId");

            migrationBuilder.RenameColumn(
                name: "last_sequence_number",
                table: "OutboxState",
                newName: "LastSequenceNumber");

            migrationBuilder.RenameColumn(
                name: "outbox_id",
                table: "OutboxState",
                newName: "OutboxId");

            migrationBuilder.RenameIndex(
                name: "ix_outbox_state_created",
                table: "OutboxState",
                newName: "IX_OutboxState_Created");

            migrationBuilder.RenameColumn(
                name: "properties",
                table: "OutboxMessage",
                newName: "Properties");

            migrationBuilder.RenameColumn(
                name: "headers",
                table: "OutboxMessage",
                newName: "Headers");

            migrationBuilder.RenameColumn(
                name: "body",
                table: "OutboxMessage",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "source_address",
                table: "OutboxMessage",
                newName: "SourceAddress");

            migrationBuilder.RenameColumn(
                name: "sent_time",
                table: "OutboxMessage",
                newName: "SentTime");

            migrationBuilder.RenameColumn(
                name: "response_address",
                table: "OutboxMessage",
                newName: "ResponseAddress");

            migrationBuilder.RenameColumn(
                name: "request_id",
                table: "OutboxMessage",
                newName: "RequestId");

            migrationBuilder.RenameColumn(
                name: "outbox_id",
                table: "OutboxMessage",
                newName: "OutboxId");

            migrationBuilder.RenameColumn(
                name: "message_type",
                table: "OutboxMessage",
                newName: "MessageType");

            migrationBuilder.RenameColumn(
                name: "message_id",
                table: "OutboxMessage",
                newName: "MessageId");

            migrationBuilder.RenameColumn(
                name: "initiator_id",
                table: "OutboxMessage",
                newName: "InitiatorId");

            migrationBuilder.RenameColumn(
                name: "inbox_message_id",
                table: "OutboxMessage",
                newName: "InboxMessageId");

            migrationBuilder.RenameColumn(
                name: "inbox_consumer_id",
                table: "OutboxMessage",
                newName: "InboxConsumerId");

            migrationBuilder.RenameColumn(
                name: "fault_address",
                table: "OutboxMessage",
                newName: "FaultAddress");

            migrationBuilder.RenameColumn(
                name: "expiration_time",
                table: "OutboxMessage",
                newName: "ExpirationTime");

            migrationBuilder.RenameColumn(
                name: "enqueue_time",
                table: "OutboxMessage",
                newName: "EnqueueTime");

            migrationBuilder.RenameColumn(
                name: "destination_address",
                table: "OutboxMessage",
                newName: "DestinationAddress");

            migrationBuilder.RenameColumn(
                name: "correlation_id",
                table: "OutboxMessage",
                newName: "CorrelationId");

            migrationBuilder.RenameColumn(
                name: "conversation_id",
                table: "OutboxMessage",
                newName: "ConversationId");

            migrationBuilder.RenameColumn(
                name: "content_type",
                table: "OutboxMessage",
                newName: "ContentType");

            migrationBuilder.RenameColumn(
                name: "sequence_number",
                table: "OutboxMessage",
                newName: "SequenceNumber");

            migrationBuilder.RenameIndex(
                name: "ix_outbox_message_outbox_id_sequence_number",
                table: "OutboxMessage",
                newName: "IX_OutboxMessage_OutboxId_SequenceNumber");

            migrationBuilder.RenameIndex(
                name: "ix_outbox_message_inbox_message_id_inbox_consumer_id_sequence_",
                table: "OutboxMessage",
                newName: "IX_OutboxMessage_InboxMessageId_InboxConsumerId_SequenceNumber");

            migrationBuilder.RenameIndex(
                name: "ix_outbox_message_expiration_time",
                table: "OutboxMessage",
                newName: "IX_OutboxMessage_ExpirationTime");

            migrationBuilder.RenameIndex(
                name: "ix_outbox_message_enqueue_time",
                table: "OutboxMessage",
                newName: "IX_OutboxMessage_EnqueueTime");

            migrationBuilder.RenameColumn(
                name: "received",
                table: "InboxState",
                newName: "Received");

            migrationBuilder.RenameColumn(
                name: "delivered",
                table: "InboxState",
                newName: "Delivered");

            migrationBuilder.RenameColumn(
                name: "consumed",
                table: "InboxState",
                newName: "Consumed");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "InboxState",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "InboxState",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "receive_count",
                table: "InboxState",
                newName: "ReceiveCount");

            migrationBuilder.RenameColumn(
                name: "message_id",
                table: "InboxState",
                newName: "MessageId");

            migrationBuilder.RenameColumn(
                name: "lock_id",
                table: "InboxState",
                newName: "LockId");

            migrationBuilder.RenameColumn(
                name: "last_sequence_number",
                table: "InboxState",
                newName: "LastSequenceNumber");

            migrationBuilder.RenameColumn(
                name: "expiration_time",
                table: "InboxState",
                newName: "ExpirationTime");

            migrationBuilder.RenameColumn(
                name: "consumer_id",
                table: "InboxState",
                newName: "ConsumerId");

            migrationBuilder.RenameIndex(
                name: "ix_inbox_state_delivered",
                table: "InboxState",
                newName: "IX_InboxState_Delivered");

            migrationBuilder.AlterColumn<DateTime>(
                name: "complited_at",
                table: "maintenance_log",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_maintenance_log",
                table: "maintenance_log",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_issues",
                table: "issues",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_equipments_projection",
                table: "equipments_projection",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_equipment_status",
                table: "equipment_status",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxState",
                table: "OutboxState",
                column: "OutboxId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessage",
                table: "OutboxMessage",
                column: "SequenceNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_InboxState_MessageId_ConsumerId",
                table: "InboxState",
                columns: new[] { "MessageId", "ConsumerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_InboxState",
                table: "InboxState",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_issues_equipment_status_EquipmentStatusId",
                table: "issues",
                column: "EquipmentStatusId",
                principalTable: "equipment_status",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_issues_equipment_status_status_id",
                table: "issues",
                column: "status_id",
                principalTable: "equipment_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_issues_maintenance_log_resolved_by_maintenance_id",
                table: "issues",
                column: "resolved_by_maintenance_id",
                principalTable: "maintenance_log",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutboxMessage_InboxState_InboxMessageId_InboxConsumerId",
                table: "OutboxMessage",
                columns: new[] { "InboxMessageId", "InboxConsumerId" },
                principalTable: "InboxState",
                principalColumns: new[] { "MessageId", "ConsumerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OutboxMessage_OutboxState_OutboxId",
                table: "OutboxMessage",
                column: "OutboxId",
                principalTable: "OutboxState",
                principalColumn: "OutboxId");
        }
    }
}
