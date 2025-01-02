using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DotNetCore_New.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToStudentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentAddress",
                table: "studentsDBTable");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DOB",
                table: "studentsDBTable",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "studentsDBTable",
                columns: new[] { "StudentId", "DOB", "StudentEmail", "StudentName", "StudentPhone" },
                values: new object[,]
                {
                    { 1, new DateTime(1995, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "saketkumar180@gmail.com", "Saket Jha", "9177881115" },
                    { 2, new DateTime(1994, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "subhamsingh@gmail.com", "Subham singh", "8437074075" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "studentsDBTable",
                keyColumn: "StudentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "studentsDBTable",
                keyColumn: "StudentId",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "DOB",
                table: "studentsDBTable",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "StudentAddress",
                table: "studentsDBTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
