using ZonkGameCore.Model;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Status of the end of the course
    /// </summary>
    public class EndTurnState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public override async Task<StateResponseModel> HandleAsync()
        {
            // Plus points for the course in the general account of the player
            _fsm.GameContext.CurrentPlayer.AddingTotalScore();

            await _observer.EndTurn();

            if (_fsm.GameContext.CurrentPlayer.TurnsCount == _fsm.GameContext.GetOpponentPlayer().TurnsCount)
            {
                // Checking for the end of the game
                if (_fsm.GameContext.Players.Any(p => p.TotalScore >= _fsm.GameContext.TargetScore))
                {
                    // The game is over, one of the players has reached the target account
                    _fsm.TransitionTo<GameOverState>();

                    return new StateResponseModel();
                }
                _fsm.RoundCount++;
            }

            // The game is not over, the move is transmitted to the next player
            _fsm.GameContext.NextPlayer();
            _fsm.TransitionTo<StartTurnState>();

            return new StateResponseModel();
        }
    }
}
