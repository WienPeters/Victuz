using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPCasusVictuz.Migrations
{
    /// <inheritdoc />
    public partial class AddDingsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Members_BoardMemberId",
                table: "Members",
                column: "BoardMemberId",
                unique: true,
                filter: "[BoardMemberId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Members_BoardMemberId",
                table: "Members",
                column: "BoardMemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Members_BoardMemberId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_BoardMemberId",
                table: "Members");
        }
    }
}
