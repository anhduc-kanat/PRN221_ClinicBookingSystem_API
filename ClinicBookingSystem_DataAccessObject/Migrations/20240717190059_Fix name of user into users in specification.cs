using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicBookingSystem_DataAccessObject.Migrations
{
    /// <inheritdoc />
    public partial class Fixnameofuserintousersinspecification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationUser_Users_UserId",
                table: "SpecificationUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "SpecificationUser",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationUser_UserId",
                table: "SpecificationUser",
                newName: "IX_SpecificationUser_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationUser_Users_UsersId",
                table: "SpecificationUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationUser_Users_UsersId",
                table: "SpecificationUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "SpecificationUser",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationUser_UsersId",
                table: "SpecificationUser",
                newName: "IX_SpecificationUser_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationUser_Users_UserId",
                table: "SpecificationUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
