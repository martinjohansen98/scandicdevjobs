using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScandicDevJobApi.Migrations
{
    /// <inheritdoc />
    public partial class companylogoUrlToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagLevel",
                table: "Tags",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyLogoUrl",
                table: "Companies",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagLevel",
                table: "Tags");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyLogoUrl",
                table: "Companies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
