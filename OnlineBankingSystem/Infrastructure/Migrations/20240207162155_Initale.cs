using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "deposits",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "credits",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "cards",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_deposits_UserId",
                table: "deposits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_credits_UserId",
                table: "credits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_cards_UserId",
                table: "cards",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_cards_AspNetUsers_UserId",
                table: "cards",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_credits_AspNetUsers_UserId",
                table: "credits",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_deposits_AspNetUsers_UserId",
                table: "deposits",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cards_AspNetUsers_UserId",
                table: "cards");

            migrationBuilder.DropForeignKey(
                name: "FK_credits_AspNetUsers_UserId",
                table: "credits");

            migrationBuilder.DropForeignKey(
                name: "FK_deposits_AspNetUsers_UserId",
                table: "deposits");

            migrationBuilder.DropIndex(
                name: "IX_deposits_UserId",
                table: "deposits");

            migrationBuilder.DropIndex(
                name: "IX_credits_UserId",
                table: "credits");

            migrationBuilder.DropIndex(
                name: "IX_cards_UserId",
                table: "cards");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "deposits");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "credits");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "cards");
        }
    }
}
