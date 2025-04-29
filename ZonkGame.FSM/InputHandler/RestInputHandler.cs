using System.Collections.Concurrent;
using ZonkGameCore.InputParams;

namespace ZonkGameCore.InputHandler
{
    public class RestInputHandler : IInputAsyncHandler
    {
        private static readonly ConcurrentDictionary<(Guid gameId, Guid playerId), List<int>?> _diceSelections = [];
        private static readonly ConcurrentDictionary<(Guid gameId, Guid playerId), bool?> _continueDecisions = [];

        public async Task<IEnumerable<int>?> HandleSelectDiceInputAsync(
            IEnumerable<int> roll,
            Guid gameid,
            Guid playerId)
        {
            if (_diceSelections.TryGetValue((gameid, playerId), out var list))
            {
                _diceSelections.TryRemove((gameid, playerId), out _);

                return list;
            }

            return null;
        }

        public async Task<bool?> HandleShouldContinueGameInputAsync(Guid gameid, Guid playerId)
        {
            if (_continueDecisions.TryGetValue((gameid, playerId), out var decision))
            {
                _continueDecisions.TryRemove((gameid, playerId), out _);

                return decision;
            }

            return null;
        }

        public static void SetSelectedDice(Guid gameId, List<int> selectedDice, Guid playerId)
        {
            if (!_diceSelections.TryGetValue((gameId, playerId), out _))
            {
                _diceSelections.TryAdd((gameId, playerId), selectedDice);
            }
        }

        public static void SetShouldContinue(Guid gameId, bool decision, Guid playerId)
        {
            if (!_continueDecisions.TryGetValue((gameId, playerId), out _))
            {
                _continueDecisions.TryAdd((gameId, playerId), decision);
            }
        }
    }
}