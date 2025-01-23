using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCore_New.Migrations
{
    /// <inheritdoc />
    public partial class CreatingFKRolePrivilageRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RolePrivilages_RoleId",
                table: "RolePrivilages",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePrivilages_Roles",
                table: "RolePrivilages",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePrivilages_Roles",
                table: "RolePrivilages");

            migrationBuilder.DropIndex(
                name: "IX_RolePrivilages_RoleId",
                table: "RolePrivilages");
        }
    }
}
