using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalGlossary_API.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyPropsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TermId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TermId",
                table: "Categories",
                column: "TermId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Terms_TermId",
                table: "Categories",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Terms_TermId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_TermId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TermId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Categories");
        }
    }
}
