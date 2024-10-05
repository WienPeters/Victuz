using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPCasusVictuz.Migrations
{
    /// <inheritdoc />
    public partial class AApic2DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedByMemberId = table.Column<int>(type: "int", nullable: true),
                    AddedByBoardMemberId = table.Column<int>(type: "int", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_BoardMembers_AddedByBoardMemberId",
                        column: x => x.AddedByBoardMemberId,
                        principalTable: "BoardMembers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pictures_Members_AddedByMemberId",
                        column: x => x.AddedByMemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_AddedByBoardMemberId",
                table: "Pictures",
                column: "AddedByBoardMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_AddedByMemberId",
                table: "Pictures",
                column: "AddedByMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pictures");
        }
    }
}
