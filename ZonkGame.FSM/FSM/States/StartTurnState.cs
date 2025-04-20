using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние начала хода игрока
    /// </summary>
    public class StartTurnState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm), IStateName
    {
        public static string StateName => "StartTurnState";

        protected override bool Handle()
        {
            _observer.Info($"Ход игрока {_fsm.GameContext.CurrentPlayer.PlayerName}");

            _fsm.GameContext.CurrentPlayer.TurnsCount++;

            // Начало хода нового игрока
            _fsm.GameContext.CurrentPlayer.ResetDices();
            _fsm.TransitionTo(new RollDiceState(_observer, _fsm));

            return true;
        }
    }
}
