using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolutionTemplate.Infrastructure.EF.Migrations;

/// <inheritdoc />
public partial class AddOutboxConsumer : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "OutboxMessage");

        migrationBuilder.CreateTable(
            name: "Outbox",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                OccurredOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                ProcessedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Outbox", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OutboxConsumer",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OutboxConsumer", x => x.Id);
                table.ForeignKey(
                    name: "FK_OutboxConsumer_Outbox_Id",
                    column: x => x.Id,
                    principalTable: "Outbox",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Outbox_OccurredOnUtc",
            table: "Outbox",
            column: "OccurredOnUtc");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "OutboxConsumer");

        migrationBuilder.DropTable(
            name: "Outbox");

        migrationBuilder.CreateTable(
            name: "OutboxMessage",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                OccurredOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                ProcessedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OutboxMessage", x => x.Id);
            });
    }
}
