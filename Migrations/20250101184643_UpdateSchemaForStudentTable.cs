using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCore_New.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaForStudentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_studentsDBTable",
                table: "studentsDBTable");

            migrationBuilder.RenameTable(
                name: "studentsDBTable",
                newName: "Students");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "studentsDBTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_studentsDBTable",
                table: "studentsDBTable",
                column: "StudentId");
        }
    }
}
