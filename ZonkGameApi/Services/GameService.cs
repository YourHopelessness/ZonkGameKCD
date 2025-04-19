using ZonkGameApi.Response;
using ZonkGameCore.FSM;

namespace ZonkGameApi.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Обработка хода игрока
        /// </summary>
        /// <param name="roomId">Идентификатор комнаты</param>
        /// <param name="stateMachine">Текущая игра</param>
        /// <returns>Состояние игры</returns>
        Task<CurrentStateResponse> MakeStep(Guid roomId, ZonkStateMachine stateMachine);
        /// <summary>
        /// Получение состояния игры
        /// </summary>
        /// <param name="roomId">Идентификатор комнаты</param>
        /// <param name="stateMachine">Текущая игра</param>
        /// <returns>Состояние игры</returns>
        CurrentStateResponse GetState(Guid roomId, ZonkStateMachine stateMachine);
    }

    /// <summary>
    /// Сервис для игры
    /// </summary>
    public class GameService : IGameService
    {
        public CurrentStateResponse GetState(Guid roomId, ZonkStateMachine stateMachine)
        {
            var gameContext = stateMachine.GameContext;
            var currentPlayer = gameContext.CurrentPlayer;
            var opponentPlayer = gameContext.GetOpponentPlayer();
            var currentState = new CurrentStateResponse
            {
                RoomId = roomId,
                IsGameOver = stateMachine.IsGameOver,
                CurrentPlayerId = currentPlayer.PlayerId,
                CurrentPlayerName = currentPlayer.PlayerName,
                PlayerScore = currentPlayer.TotalScore + currentPlayer.TurnScore,
                OpponentScore = opponentPlayer.TotalScore,
                RemainingDice = gameContext.CurrentPlayer.RemainingDice,
                CurrentRoll = [.. gameContext.CurrentRoll],
                AvailableCombinations = gameContext.GetValidCombinations(),
                TargetScore = gameContext.TargetScore
            };

            return currentState;
        }

        public async Task<CurrentStateResponse> MakeStep(Guid roomId, ZonkStateMachine stateMachine)
        {
            _ = await stateMachine.Handle();

            return GetState(roomId, stateMachine);
        }
    }
}
