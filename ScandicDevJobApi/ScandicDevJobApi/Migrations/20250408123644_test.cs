using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScandicDevJobApi.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobListingTag_Joblistings_JobListingsId",
                table: "JobListingTag");

            migrationBuilder.RenameColumn(
                name: "JobListingsId",
                table: "JobListingTag",
                newName: "JoblistingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListingTag_Joblistings_JoblistingsId",
                table: "JobListingTag",
                column: "JoblistingsId",
                principalTable: "Joblistings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobListingTag_Joblistings_JoblistingsId",
                table: "JobListingTag");

            migrationBuilder.RenameColumn(
                name: "JoblistingsId",
                table: "JobListingTag",
                newName: "JobListingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListingTag_Joblistings_JobListingsId",
                table: "JobListingTag",
                column: "JobListingsId",
                principalTable: "Joblistings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
