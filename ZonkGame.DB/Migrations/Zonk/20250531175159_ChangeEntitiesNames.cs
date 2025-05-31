using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZonkGame.DB.Migrations.Zonk
{
    /// <inheritdoc />
    public partial class ChangeEntitiesNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameAudits_Games_GameId",
                table: "GameAudits");

            migrationBuilder.DropForeignKey(
                name: "FK_GameAudits_Players_CurrentPlayerId",
                table: "GameAudits");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_Games_GameId",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_Players_PlayerId",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Players",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameAudits",
                table: "GameAudits");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "player");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "game");

            migrationBuilder.RenameTable(
                name: "GamePlayers",
                newName: "game_player");

            migrationBuilder.RenameTable(
                name: "GameAudits",
                newName: "game_audit");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "player",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "player",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "PlayerType",
                table: "player",
                newName: "player_type");

            migrationBuilder.RenameColumn(
                name: "PlayerName",
                table: "player",
                newName: "player_name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "game",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TargetScore",
                table: "game",
                newName: "target_score");

            migrationBuilder.RenameColumn(
                name: "GameType",
                table: "game",
                newName: "game_type");

            migrationBuilder.RenameColumn(
                name: "GameState",
                table: "game",
                newName: "game_state");

            migrationBuilder.RenameColumn(
                name: "EndedAt",
                table: "game",
                newName: "ended_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "game",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "WinnerId",
                table: "game",
                newName: "winner_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "game_player",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PlayerTurn",
                table: "game_player",
                newName: "player_turn");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "game_player",
                newName: "player_id");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "game_player",
                newName: "game_id");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayers_PlayerId",
                table: "game_player",
                newName: "ix_game_player_player_id");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayers_GameId",
                table: "game_player",
                newName: "ix_game_player_game_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "game_audit",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SelectedCombination",
                table: "game_audit",
                newName: "selected_combination");

            migrationBuilder.RenameColumn(
                name: "RemainingDice",
                table: "game_audit",
                newName: "remaining_dice");

            migrationBuilder.RenameColumn(
                name: "RecordTime",
                table: "game_audit",
                newName: "record_time");

            migrationBuilder.RenameColumn(
                name: "PlayerTurnScore",
                table: "game_audit",
                newName: "player_turn_score");

            migrationBuilder.RenameColumn(
                name: "PlayerTotalScore",
                table: "game_audit",
                newName: "player_total_score");

            migrationBuilder.RenameColumn(
                name: "OpponentTotalScore",
                table: "game_audit",
                newName: "opponent_total_score");

            migrationBuilder.RenameColumn(
                name: "EventType",
                table: "game_audit",
                newName: "event_type");

            migrationBuilder.RenameColumn(
                name: "CurrentRoll",
                table: "game_audit",
                newName: "current_roll");

            migrationBuilder.RenameColumn(
                name: "AvaliableCombination",
                table: "game_audit",
                newName: "avaliable_combination");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "game_audit",
                newName: "player_id");

            migrationBuilder.RenameColumn(
                name: "CurrentPlayerId",
                table: "game_audit",
                newName: "game_id");

            migrationBuilder.RenameIndex(
                name: "IX_GameAudits_GameId",
                table: "game_audit",
                newName: "ix_game_audit_player_id");

            migrationBuilder.RenameIndex(
                name: "IX_GameAudits_CurrentPlayerId",
                table: "game_audit",
                newName: "ix_game_audit_game_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_player",
                table: "player",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_game",
                table: "game",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_game_player",
                table: "game_player",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_game_audit",
                table: "game_audit",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_game_player_winner_id",
                table: "game",
                column: "winner_id",
                principalTable: "player",
                principalColumn: "id");
            

            migrationBuilder.AddForeignKey(
                name: "fk_game_player_player_player_id",
                table: "game_player",
                column: "player_id",
                principalTable: "player",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_game_player_winner_id",
                table: "game");

            migrationBuilder.DropForeignKey(
                name: "fk_game_audit_game_game_id",
                table: "game_audit");

            migrationBuilder.DropForeignKey(
                name: "fk_game_audit_player_player_id",
                table: "game_audit");

            migrationBuilder.DropForeignKey(
                name: "fk_game_player_game_game_id",
                table: "game_player");

            migrationBuilder.DropForeignKey(
                name: "fk_game_player_player_player_id",
                table: "game_player");

            migrationBuilder.DropPrimaryKey(
                name: "pk_player",
                table: "player");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game_player",
                table: "game_player");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game_audit",
                table: "game_audit");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game",
                table: "game");

            migrationBuilder.RenameTable(
                name: "player",
                newName: "Players");

            migrationBuilder.RenameTable(
                name: "game_player",
                newName: "GamePlayers");

            migrationBuilder.RenameTable(
                name: "game_audit",
                newName: "GameAudits");

            migrationBuilder.RenameTable(
                name: "game",
                newName: "Games");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Players",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Players",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "player_type",
                table: "Players",
                newName: "PlayerType");

            migrationBuilder.RenameColumn(
                name: "player_name",
                table: "Players",
                newName: "PlayerName");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "GamePlayers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "player_turn",
                table: "GamePlayers",
                newName: "PlayerTurn");

            migrationBuilder.RenameColumn(
                name: "player_id",
                table: "GamePlayers",
                newName: "PlayerId");

            migrationBuilder.RenameColumn(
                name: "game_id",
                table: "GamePlayers",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "ix_game_player_player_id",
                table: "GamePlayers",
                newName: "IX_GamePlayers_PlayerId");

            migrationBuilder.RenameIndex(
                name: "ix_game_player_game_id",
                table: "GamePlayers",
                newName: "IX_GamePlayers_GameId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "GameAudits",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "selected_combination",
                table: "GameAudits",
                newName: "SelectedCombination");

            migrationBuilder.RenameColumn(
                name: "remaining_dice",
                table: "GameAudits",
                newName: "RemainingDice");

            migrationBuilder.RenameColumn(
                name: "record_time",
                table: "GameAudits",
                newName: "RecordTime");

            migrationBuilder.RenameColumn(
                name: "player_turn_score",
                table: "GameAudits",
                newName: "PlayerTurnScore");

            migrationBuilder.RenameColumn(
                name: "player_total_score",
                table: "GameAudits",
                newName: "PlayerTotalScore");

            migrationBuilder.RenameColumn(
                name: "opponent_total_score",
                table: "GameAudits",
                newName: "OpponentTotalScore");

            migrationBuilder.RenameColumn(
                name: "event_type",
                table: "GameAudits",
                newName: "EventType");

            migrationBuilder.RenameColumn(
                name: "current_roll",
                table: "GameAudits",
                newName: "CurrentRoll");

            migrationBuilder.RenameColumn(
                name: "avaliable_combination",
                table: "GameAudits",
                newName: "AvaliableCombination");

            migrationBuilder.RenameColumn(
                name: "player_id",
                table: "GameAudits",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "game_id",
                table: "GameAudits",
                newName: "CurrentPlayerId");

            migrationBuilder.RenameIndex(
                name: "ix_game_audit_player_id",
                table: "GameAudits",
                newName: "IX_GameAudits_GameId");

            migrationBuilder.RenameIndex(
                name: "ix_game_audit_game_id",
                table: "GameAudits",
                newName: "IX_GameAudits_CurrentPlayerId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Games",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "target_score",
                table: "Games",
                newName: "TargetScore");

            migrationBuilder.RenameColumn(
                name: "game_type",
                table: "Games",
                newName: "GameType");

            migrationBuilder.RenameColumn(
                name: "game_state",
                table: "Games",
                newName: "GameState");

            migrationBuilder.RenameColumn(
                name: "ended_at",
                table: "Games",
                newName: "EndedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Games",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "winner_id",
                table: "Games",
                newName: "WinnerId");

            migrationBuilder.RenameIndex(
                name: "ix_game_winner_id",
                table: "Games",
                newName: "IX_Games_WinnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameAudits",
                table: "GameAudits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameAudits_Games_GameId",
                table: "GameAudits",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameAudits_Players_CurrentPlayerId",
                table: "GameAudits",
                column: "CurrentPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_Games_GameId",
                table: "GamePlayers",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_Players_PlayerId",
                table: "GamePlayers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_WinnerId",
                table: "Games",
                column: "WinnerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
