using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolutionTemplate.Infrastructure.EF.Migrations;

/// <inheritdoc />
public partial class OutboxTweaks : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_OutboxConsumer_Outbox_Id",
            table: "OutboxConsumer");

        migrationBuilder.DropPrimaryKey(
            name: "PK_OutboxConsumer",
            table: "OutboxConsumer");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "OutboxConsumer",
            newName: "OutboxMessageId");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "OutboxConsumer",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Type",
            table: "Outbox",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Content",
            table: "Outbox",
            type: "nvarchar(2000)",
            maxLength: 2000,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddPrimaryKey(
            name: "PK_OutboxConsumer",
            table: "OutboxConsumer",
            columns: new[] { "OutboxMessageId", "Name" });

        migrationBuilder.AddForeignKey(
            name: "FK_OutboxConsumer_Outbox_OutboxMessageId",
            table: "OutboxConsumer",
            column: "OutboxMessageId",
            principalTable: "Outbox",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_OutboxConsumer_Outbox_OutboxMessageId",
            table: "OutboxConsumer");

        migrationBuilder.DropPrimaryKey(
            name: "PK_OutboxConsumer",
            table: "OutboxConsumer");

        migrationBuilder.RenameColumn(
            name: "OutboxMessageId",
            table: "OutboxConsumer",
            newName: "Id");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "OutboxConsumer",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(400)",
            oldMaxLength: 400);

        migrationBuilder.AlterColumn<string>(
            name: "Type",
            table: "Outbox",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(400)",
            oldMaxLength: 400);

        migrationBuilder.AlterColumn<string>(
            name: "Content",
            table: "Outbox",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(2000)",
            oldMaxLength: 2000);

        migrationBuilder.AddPrimaryKey(
            name: "PK_OutboxConsumer",
            table: "OutboxConsumer",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_OutboxConsumer_Outbox_Id",
            table: "OutboxConsumer",
            column: "Id",
            principalTable: "Outbox",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}