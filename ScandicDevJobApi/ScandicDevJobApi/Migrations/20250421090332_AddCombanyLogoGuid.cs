using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScandicDevJobApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCombanyLogoGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLogoUrl",
                table: "Companies");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyLogoGuid",
                table: "Companies",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLogoGuid",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogoUrl",
                table: "Companies",
                type: "text",
                nullable: true);
        }
    }
}
