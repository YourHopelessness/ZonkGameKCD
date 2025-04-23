using ZonkGameCore.InputParams;

public class RestInputHandler : IInputAsyncHandler
{
    private static readonly Dictionary<Guid, TaskCompletionSource<List<int>>> _diceSelections = [];
    private static readonly Dictionary<Guid, TaskCompletionSource<bool>> _continueDecisions = [];

    public async Task<IEnumerable<int>> HandleSelectDiceInputAsync(IEnumerable<int> roll, Guid gameid)
    {
        var tcs = new TaskCompletionSource<List<int>>();
        _diceSelections[gameid] = tcs;

        return await tcs.Task;
    }

    public async Task<bool> HandleShouldContinueGameInputAsync(Guid gameid)
    {
        var tcs = new TaskCompletionSource<bool>();
        _continueDecisions[gameid] = tcs;

        return await tcs.Task;
    }

    public static void SetSelectedDice(Guid gameId, List<int> selectedDice)
    {
        if (_diceSelections.TryGetValue(gameId, out var tcs))
        {
            tcs.SetResult(selectedDice);
            _diceSelections.Remove(gameId);
        }
    }

    public static void SetShouldContinue(Guid gameId, bool decision)
    {
        if (_continueDecisions.TryGetValue(gameId, out var tcs))
        {
            tcs.SetResult(decision);
            _continueDecisions.Remove(gameId);
        }
    }
}
