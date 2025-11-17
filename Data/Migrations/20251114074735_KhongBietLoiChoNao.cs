using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NStudents.Migrations
{
    /// <inheritdoc />
    public partial class KhongBietLoiChoNao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_Students_StudentId",
                table: "users");

            migrationBuilder.AddForeignKey(
                name: "FK_users_Students_StudentId",
                table: "users",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_Students_StudentId",
                table: "users");

            migrationBuilder.AddForeignKey(
                name: "FK_users_Students_StudentId",
                table: "users",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
