using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mooc.Model.Migrations
{
    /// <inheritdoc />
    public partial class testdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "User",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "User");
        }
    }
}
