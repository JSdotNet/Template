using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolutionTemplate.Infrastructure.EF.Migrations;

/// <inheritdoc />
public partial class SimpleTags : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Tag");

        migrationBuilder.AddColumn<string>(
            name: "Tags",
            table: "Articles",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Tags",
            table: "Articles");

        migrationBuilder.CreateTable(
            name: "Tag",
            columns: table => new
            {
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tag", x => x.Name);
                table.ForeignKey(
                    name: "FK_Tag_Articles_ArticleId",
                    column: x => x.ArticleId,
                    principalTable: "Articles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Tag_ArticleId",
            table: "Tag",
            column: "ArticleId");
    }
}