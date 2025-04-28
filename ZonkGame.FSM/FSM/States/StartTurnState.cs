using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние начала хода игрока
    /// </summary>
    public class StartTurnState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public override async Task HandleAsync()
        {
            await _observer.NewTurn();

            _fsm.GameContext.CurrentPlayer.TurnsCount++;

            // Начало хода нового игрока
            _fsm.GameContext.CurrentPlayer.ResetDices();
            _fsm.TransitionTo<RollDiceState>();
        }
    }
}
