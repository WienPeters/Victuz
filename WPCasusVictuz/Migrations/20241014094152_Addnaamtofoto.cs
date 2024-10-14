using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPCasusVictuz.Migrations
{
    /// <inheritdoc />
    public partial class Addnaamtofoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Members_AddedByMemberId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_AddedByMemberId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "AddedByMemberId",
                table: "Pictures");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Pictures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Pictures");

            migrationBuilder.AddColumn<int>(
                name: "AddedByMemberId",
                table: "Pictures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_AddedByMemberId",
                table: "Pictures",
                column: "AddedByMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Members_AddedByMemberId",
                table: "Pictures",
                column: "AddedByMemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }
    }
}
