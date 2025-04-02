using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ScandicDevJobApi.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedJobListingAndMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SalaryRangeMin",
                table: "Joblistings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "SalaryRangeMax",
                table: "Joblistings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Joblistings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Joblistings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Joblistings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Joblistings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ApplicationDeadline",
                table: "Joblistings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUrl",
                table: "Joblistings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Joblistings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Joblistings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Joblistings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Joblistings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Joblistings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmploymentType",
                table: "Joblistings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Joblistings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "JobListingExpiryDate",
                table: "Joblistings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Joblistings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Joblistings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfApplicants",
                table: "Joblistings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Joblistings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkMode",
                table: "Joblistings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyLogoUrl = table.Column<int>(type: "integer", nullable: true),
                    CompanySize = table.Column<int>(type: "integer", nullable: true),
                    Website = table.Column<string>(type: "text", nullable: true),
                    ContactEmail = table.Column<string>(type: "text", nullable: true),
                    ContactPhone = table.Column<string>(type: "text", nullable: true),
                    LinkedIn = table.Column<string>(type: "text", nullable: true),
                    Twitter = table.Column<string>(type: "text", nullable: true),
                    Facebook = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UnicodeIcon = table.Column<string>(type: "text", nullable: true),
                    TagCategory = table.Column<int>(type: "integer", nullable: true),
                    JobListingId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Joblistings_JobListingId",
                        column: x => x.JobListingId,
                        principalTable: "Joblistings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Joblistings_CompanyId",
                table: "Joblistings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_JobListingId",
                table: "Tags",
                column: "JobListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Joblistings_Companies_CompanyId",
                table: "Joblistings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Joblistings_Companies_CompanyId",
                table: "Joblistings");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Joblistings_CompanyId",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "ApplicationDeadline",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "ApplicationUrl",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "JobListingExpiryDate",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "NumberOfApplicants",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Joblistings");

            migrationBuilder.DropColumn(
                name: "WorkMode",
                table: "Joblistings");

            migrationBuilder.AlterColumn<int>(
                name: "SalaryRangeMin",
                table: "Joblistings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SalaryRangeMax",
                table: "Joblistings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Joblistings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Joblistings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Joblistings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Joblistings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
