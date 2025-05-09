﻿using ZonkGameCore.Dto;
using ZonkGameCore.Observer;

namespace ZonkGameCore.FSM.States
{
    /// <summary>
    /// Состояние продолжения хода
    /// </summary>
    public class AskContinueState(BaseObserver observer, ZonkStateMachine fsm) : BaseGameState(observer, fsm)
    {
        public async override Task<StateResponse> HandleAsync()
        {
            // Вопрос о продолжении игры
            var desision = await _fsm.GameContext.CurrentPlayer.PlayerInputHandler
                .HandleShouldContinueGameInputAsync(_fsm.GameId, _fsm.GameContext.CurrentPlayer.PlayerId);

            if (desision == null)
            {
                return new StateResponse()
                {
                    TransitToNewState = false,
                    NeedContinueDecision = true
                };
            }
            else if (desision.Value)
            {
                await _observer.ContinueTurn();
                _fsm.TransitionTo<RollDiceState>();
            }
            else
            {
                await _observer.FinishTurn();
                _fsm.TransitionTo<EndTurnState>();
            }

            return new StateResponse();
        }
    }
}
