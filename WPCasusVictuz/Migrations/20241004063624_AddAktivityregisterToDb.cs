using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPCasusVictuz.Migrations
{
    /// <inheritdoc />
    public partial class AddAktivityregisterToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedbyBM",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CreatedbyBM",
                table: "Activities",
                column: "CreatedbyBM");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_BoardMembers_CreatedbyBM",
                table: "Activities",
                column: "CreatedbyBM",
                principalTable: "BoardMembers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_BoardMembers_CreatedbyBM",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_CreatedbyBM",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CreatedbyBM",
                table: "Activities");
        }
    }
}
