using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DotNetCore_New.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewEntriesToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "DOB", "StudentEmail", "StudentName", "StudentPhone" },
                values: new object[,]
                {
                    { 3, new DateTime(1994, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "tinkusingh@gmail.com", "Tinku Singh", "8978246007" },
                    { 4, new DateTime(1994, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "aruntogi@gmail.com", "Arun Togi", "8897534078" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 4);
        }
    }
}
