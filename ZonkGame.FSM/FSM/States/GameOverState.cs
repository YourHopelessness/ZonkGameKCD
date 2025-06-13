using ZonkGameCore.Model;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// The end of the end of the game
    /// </summary>
    public class GameOverState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public override async Task<StateResponseModel> HandleAsync()
        {
            // The flag of the end of the game
            _fsm.SetGameOver();

            // Definition of the winner
            var winner = _fsm.GameContext.Players.FirstOrDefault(p => p.TotalScore >= _fsm.GameContext.TargetScore)
                ?? throw new InvalidOperationException("The end of the game is impossible without achieving one of the players of the target account");
            winner.IsWinner = true;

            await _observer.EndGame(winner.PlayerId, winner.PlayerName, winner.TotalScore);

            return new StateResponseModel();
        }
    }
}
