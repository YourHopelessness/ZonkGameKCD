using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние окончания игры
    /// </summary>
    public class GameOverState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public override async Task HandleAsync()
        {
            // Флаг конца игры
            _fsm.IsGameOver = true;

            // Определение победителя
            var winner = _fsm.GameContext.Players.FirstOrDefault(p => p.TotalScore >= _fsm.GameContext.TargetScore)
                ?? throw new InvalidOperationException("Конец игры невозможен без достижения одним из игроков целевого счета");
            winner.IsWinner = true;

            await _observer.EndGame(winner.PlayerName, winner.TotalScore);
        }
    }
}
