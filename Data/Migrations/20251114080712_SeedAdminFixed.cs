using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NStudents.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "UserId", "PasswordHash", "Role", "StudentId", "Username" },
                values: new object[] { 2, "$2a$11$SrcZhF8jwQKBmzMqTvsLZOro7b3/PFVHNuLLC3Kg5hd4314wNyLNK", "Admin", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 2);
        }
    }
}
