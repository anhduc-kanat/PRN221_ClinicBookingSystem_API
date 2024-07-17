using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicBookingSystem_DataAccessObject.Migrations
{
    /// <inheritdoc />
    public partial class specification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specifications_Users_UserId",
                table: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Specifications_UserId",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "AwaredAt",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "DateOfIssue",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Specifications");

            migrationBuilder.AddColumn<int>(
                name: "SpecificationId",
                table: "BusinessServices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpecificationUser",
                columns: table => new
                {
                    SpecificationsId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationUser", x => new { x.SpecificationsId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SpecificationUser_Specifications_SpecificationsId",
                        column: x => x.SpecificationsId,
                        principalTable: "Specifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecificationUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessServices_SpecificationId",
                table: "BusinessServices",
                column: "SpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationUser_UserId",
                table: "SpecificationUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessServices_Specifications_SpecificationId",
                table: "BusinessServices",
                column: "SpecificationId",
                principalTable: "Specifications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessServices_Specifications_SpecificationId",
                table: "BusinessServices");

            migrationBuilder.DropTable(
                name: "SpecificationUser");

            migrationBuilder.DropIndex(
                name: "IX_BusinessServices_SpecificationId",
                table: "BusinessServices");

            migrationBuilder.DropColumn(
                name: "SpecificationId",
                table: "BusinessServices");

            migrationBuilder.AddColumn<string>(
                name: "AwaredAt",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfIssue",
                table: "Specifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Specifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_UserId",
                table: "Specifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specifications_Users_UserId",
                table: "Specifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
