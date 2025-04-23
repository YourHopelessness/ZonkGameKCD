using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние окончания хода
    /// </summary>
    public class EndTurnState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        protected override bool Handle()
        {
            // Плюсуем очки за ход в общий счет игрока
            _fsm.GameContext.CurrentPlayer.AddingTotalScore();

            _observer.Info($"Ход игрока {_fsm.GameContext.CurrentPlayer.PlayerName} " +
                $"закончен со счетом {_fsm.GameContext.CurrentPlayer.TurnScore}. " +
                $"Суммарный счет {_fsm.GameContext.CurrentPlayer.TotalScore}");

            // Проверка на конец игры
            if (_fsm.GameContext.CurrentPlayer.TurnsCount == _fsm.GameContext.GetOpponentPlayer().TurnsCount)
            {
                if (_fsm.GameContext.Players.Any(p => p.TotalScore >= _fsm.GameContext.TargetScore))
                {
                    // Игра окончена, кто-то из игроков достиг целевого счета
                    _fsm.TransitionTo(new GameOverState(_observer, _fsm));

                    return true;
                }
                _fsm.RoundCount++;
            }

            // Игра не окончена, ход передается следующему игроку
            _fsm.GameContext.NextPlayer();
            _fsm.TransitionTo(new StartTurnState(_observer, _fsm));
            
            return true;
        }
    }
}
