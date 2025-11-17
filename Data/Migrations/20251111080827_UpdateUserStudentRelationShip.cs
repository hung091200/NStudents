using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NStudents.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserStudentRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_StudentId",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_StudentId",
                table: "users",
                column: "StudentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_StudentId",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "users",
                newName: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_users_StudentId",
                table: "users",
                column: "StudentId");
        }
    }
}
