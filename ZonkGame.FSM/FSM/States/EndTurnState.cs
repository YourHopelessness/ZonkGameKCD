using ZonkGameCore.Model;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние окончания хода
    /// </summary>
    public class EndTurnState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public override async Task<StateResponseModel> HandleAsync()
        {
            // Плюсуем очки за ход в общий счет игрока
            _fsm.GameContext.CurrentPlayer.AddingTotalScore();

            await _observer.EndTurn();

            if (_fsm.GameContext.CurrentPlayer.TurnsCount == _fsm.GameContext.GetOpponentPlayer().TurnsCount)
            {
                // Проверка на конец игры
                if (_fsm.GameContext.Players.Any(p => p.TotalScore >= _fsm.GameContext.TargetScore))
                {
                    // Игра окончена, кто-то из игроков достиг целевого счета
                    _fsm.TransitionTo<GameOverState>();

                    return new StateResponseModel();
                }
                _fsm.RoundCount++;
            }

            // Игра не окончена, ход передается следующему игроку
            _fsm.GameContext.NextPlayer();
            _fsm.TransitionTo<StartTurnState>();

            return new StateResponseModel();
        }
    }
}
