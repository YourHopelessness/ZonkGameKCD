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
            _observer.Info($"Игрок {_fsm.GameContext.CurrentPlayer.PlayerName}, хотите продолжить игру? (y/n)");

            // Вопрос о продолжении игры
            var desision = await _fsm.GameContext.CurrentPlayer.PlayerInputHandler.HandleShouldContinueGameInputAsync();

            if (desision)
            {
                _fsm.TransitionTo(new RollDiceState(_observer, _fsm));
            }
            else
            {
                _fsm.TransitionTo(new EndTurnState(_observer, _fsm));
            }
        }
    }
}
