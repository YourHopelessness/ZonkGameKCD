using ZonkGameCore.InputParams;

public class RestInputHandler(Guid gameId) : IInputAsyncHandler
{
    private static readonly Dictionary<Guid, TaskCompletionSource<List<int>>> _diceSelections = [];
    private static readonly Dictionary<Guid, TaskCompletionSource<bool>> _continueDecisions = [];
    private readonly Guid _gameId = gameId;

    public async Task<IEnumerable<int>> HandleSelectDiceInputAsync(IEnumerable<int> roll)
    {
        var tcs = new TaskCompletionSource<List<int>>();
        _diceSelections[_gameId] = tcs;

        return await tcs.Task;
    }

    public async Task<bool> HandleShouldContinueGameInputAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        _continueDecisions[_gameId] = tcs;

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
