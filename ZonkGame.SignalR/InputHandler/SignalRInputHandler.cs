using Microsoft.AspNetCore.SignalR;
using ZonkGameApi.Hubs;
using ZonkGameCore.InputParams;

namespace ZonkGameSignalR.InputHandler
{
    public class SignalRInputHandler : IInputAsyncHandler
    {
        private readonly ZonkGameHub _context;

        public SignalRInputHandler(
            ZonkGameHub context)
        {
            _context = context;
        }

        public async Task<IEnumerable<int>?> HandleSelectDiceInputAsync(IEnumerable<int> roll, Guid gameid, Guid playerId)
        {
            if (_context.Clients == null)
            {
                throw new InvalidOperationException("Clients is null");
            }

            var tcs = new TaskCompletionSource<List<int>>();
            _context.DiceSelections[_context.Context.ConnectionId] = tcs;

            await _context.Clients.Client(_context.Context.ConnectionId).SendAsync("HandleSelectDiceInputAsync", roll);

            return await tcs.Task;
        }

        public async Task<bool?> HandleShouldContinueGameInputAsync(Guid gameid, Guid playerId)
        {
            if (_context.Clients == null)
            {
                throw new InvalidOperationException("Clients is null");
            }

            var tcs = new TaskCompletionSource<bool>();
            _context.ContinueDecisions[_context.Context.ConnectionId] = tcs;

            await _context.Clients.Client(_context.Context.ConnectionId).SendAsync("HandleShouldContinueGameInputAsync");

            return await tcs.Task;
        }
    }
}
