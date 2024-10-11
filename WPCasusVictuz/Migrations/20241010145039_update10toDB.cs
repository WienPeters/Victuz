using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPCasusVictuz.Migrations
{
    /// <inheritdoc />
    public partial class update10toDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Registrations_MemberId",
                table: "Registrations");

            migrationBuilder.AddColumn<int>(
                name: "CurrentParticipants",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_MemberId_AktivityId",
                table: "Registrations",
                columns: new[] { "MemberId", "AktivityId" },
                unique: true,
                filter: "[MemberId] IS NOT NULL AND [AktivityId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Registrations_MemberId_AktivityId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "CurrentParticipants",
                table: "Activities");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_MemberId",
                table: "Registrations",
                column: "MemberId");
        }
    }
}
