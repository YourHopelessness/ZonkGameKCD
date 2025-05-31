using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZonkGame.DB.Migrations.Zonk
{
    /// <inheritdoc />
    public partial class ChangeNewNamesEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "player_id",
                table: "game_audit",
                newName: "current_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_game_audit_player_id",
                table: "game_audit",
                newName: "ix_game_audit_current_player_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_game_audit_player_current_player_id",
                table: "game_audit");

            migrationBuilder.RenameColumn(
                name: "current_player_id",
                table: "game_audit",
                newName: "player_id");

            migrationBuilder.RenameIndex(
                name: "ix_game_audit_current_player_id",
                table: "game_audit",
                newName: "ix_game_audit_player_id");

            migrationBuilder.AddForeignKey(
                name: "fk_game_audit_player_player_id",
                table: "game_audit",
                column: "player_id",
                principalTable: "player",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
