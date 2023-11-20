using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolutionTemplate.Infrastructure.EF.Migrations;

/// <inheritdoc />
public partial class AddOutboxIndex : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_Outbox_ProcessedDateUtc",
            table: "Outbox",
            column: "ProcessedDateUtc");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Outbox_ProcessedDateUtc",
            table: "Outbox");
    }
}