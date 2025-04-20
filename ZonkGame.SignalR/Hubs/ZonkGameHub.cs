using Microsoft.AspNetCore.SignalR;

namespace ZonkGameApi.Hubs
{
    public class ZonkGameHub : Hub
    {
        public Dictionary<string, TaskCompletionSource<List<int>>> DiceSelections { get; set; } = [];
        public Dictionary<string, TaskCompletionSource<bool>> ContinueDecisions { get; set; } = [];

        public async Task SendSelectedDice(List<int> selectedDice)
        {
            var connectionId = Context.ConnectionId;
            await ReceiveSelectedDice(connectionId, selectedDice);
        }

        public async Task SendContinueDecision(bool decision)
        {
            var connectionId = Context.ConnectionId;
            await ReceiveShouldContinue(connectionId, decision);
        }

        public Task ReceiveShouldContinue(string connectionId, bool decision)
        {
            if (ContinueDecisions.TryGetValue(connectionId, out var tcs))
            {
                tcs.SetResult(decision);
                DiceSelections.Remove(connectionId);
            }

            return Task.CompletedTask;
        }

        public Task ReceiveSelectedDice(string connectionId, List<int> selectedDice)
        {
            if (DiceSelections.TryGetValue(connectionId, out var tcs))
            {
                tcs.SetResult(selectedDice);
                DiceSelections.Remove(connectionId);
            }

            return Task.CompletedTask;
        }
    }
}