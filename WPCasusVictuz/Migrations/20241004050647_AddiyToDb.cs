using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPCasusVictuz.Migrations
{
    /// <inheritdoc />
    public partial class AddiyToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Members_BoardMemberId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Members_CreatedByBoardMemberId",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Members_BoardMemberId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Members");

            migrationBuilder.CreateTable(
                name: "BoardMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardMembers_MemberId",
                table: "BoardMembers",
                column: "MemberId",
                unique: true,
                filter: "[MemberId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_BoardMembers_CreatedByBoardMemberId",
                table: "Polls",
                column: "CreatedByBoardMemberId",
                principalTable: "BoardMembers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_BoardMembers_CreatedByBoardMemberId",
                table: "Polls");

            migrationBuilder.DropTable(
                name: "BoardMembers");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Members",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_Members_CreatedByBoardMemberId",
                table: "Polls",
                column: "CreatedByBoardMemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }
    }
}
