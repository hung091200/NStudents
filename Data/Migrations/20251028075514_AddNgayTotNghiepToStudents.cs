using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NStudents.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNgayTotNghiepToStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTotNghiep",
                table: "Students",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayTotNghiep",
                table: "Students");
        }
    }
}
