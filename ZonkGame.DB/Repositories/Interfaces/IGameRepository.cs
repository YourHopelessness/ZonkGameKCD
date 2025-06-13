using ZonkGame.DB.Entites.Zonk;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Repositories.Interfaces
{
    public interface IGameRepository
    {
        /// <summary>Creating a new game</summary>
        Task CreateNewGameAsync(
            Guid gameId,
            int targetScore,
            List<Guid> players,
            ModesEnum gameType,
            string initState);

        /// <summary>Updating the state of the game</summary>
        Task UpdateGameStateAsync(Guid gameId, string newState);

        /// <summary>Obtaining a game by identifier</summary>
        Task<Game?> GetGameByIdAsync(Guid gameId);

        /// <summary>The appointment of the winner of the game</summary>
        Task SetGameWinner(Guid gameId, Guid playerId);

        /// <summary> receipt of a player-livaries by their identifiers
        Task<GamePlayer> GetGamePlayerAsync(Guid gameId, Guid playerId);

        /// <summary>Obtaining a player by identifier</summary>
        Task<Player?> GetPlayerAsync(Guid playerId);

        /// <summary>Obtaining a player by name</summary>
        Task<Player?> GetPlayerByNameAsync(string playerName);

        /// <summary>Creation or updating the player</summary>
        Task<Player> CreateOrUpdatePlayerAsync(Player player);

        /// <summary>Removing games</summary>
        Task DeleteGamesAsync(List<Game> games);

        /// <summary>Search for all incomplete games</summary>
        Task<List<Game>> GetAllNotFinishedGames();

        /// <summary>Complete the game without the winner ahead of schedule</summary>
        Task FinishGameAsync(Guid gameId);
    }
}
