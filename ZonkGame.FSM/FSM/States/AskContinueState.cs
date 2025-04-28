using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние продолжения хода
    /// </summary>
    public class AskContinueState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public async override Task HandleAsync()
        {
            // Вопрос о продолжении игры
            var desision = await _fsm.GameContext.CurrentPlayer.PlayerInputHandler
                .HandleShouldContinueGameInputAsync(_fsm.GameId);

            if (desision)
            {
                _observer.ContinueTurn();
                _fsm.TransitionTo<RollDiceState>();
            }
            else
            {
                _observer.FinishTurn();
                _fsm.TransitionTo<EndTurnState>();
            }
        }
    }
}
