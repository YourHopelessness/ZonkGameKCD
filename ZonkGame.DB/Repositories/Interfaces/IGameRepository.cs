using ZonkGame.DB.Entites;
using ZonkGame.DB.Enum;

namespace ZonkGame.DB.Repositories.Interfaces
{
    public interface IGameRepository
    {
        /// <summary> Создание новой игры </summary>
        Task CreateNewGameAsync(
            Guid gameId,
            int targetScore,
            List<Guid> players,
            ModesEnum gameType,
            string initState);

        /// <summary> Обновление состояния игры </summary>
        Task UpdateGameStateAsync(Guid gameId, string newState);

        /// <summary> Получение игры по идентификатору </summary>
        Task<Game?> GetGameByIdAsync(Guid gameId);

        /// <summary> Названичение победителя игры </summary>
        Task SetGameWinner(Guid gameId, Guid playerId);

        /// <summary> Получени игрока-связи по их идентификаторам
        Task<GamePlayer> GetGamePlayerAsync(Guid gameId, Guid playerId);

        /// <summary> Получение игрока по идентификатору </summary>
        Task<Player?> GetPlayerAsync(Guid playerId);

        /// <summary> Получение игрока по имени </summary>
        Task<Player?> GetPlayerByNameAsync(string playerName);

        /// <summary> Создание или обновление игрока </summary>
        Task<Player> CreateOrUpdatePlayerAsync(Player player);

        /// <summary> Удаление игр </summary>
        Task DeleteGamesAsync(List<Game> games);

        /// <summary> Поиск всех незавершенных игр </summary>
        Task<List<Game>> GetAllNotFinishedGames();

        /// <summary> Завершить игру без победителя досрочно </summary>
        Task FinishGame(Guid gameId);
    }
}
