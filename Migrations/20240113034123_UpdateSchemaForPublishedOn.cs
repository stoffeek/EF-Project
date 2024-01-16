using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFpro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaForPublishedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published_On",
                table: "Posts");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedOn",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedOn",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "Published_On",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
