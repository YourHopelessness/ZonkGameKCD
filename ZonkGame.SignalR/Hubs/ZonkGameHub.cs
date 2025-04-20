using Microsoft.AspNetCore.SignalR;

namespace ZonkGameApi.Hubs
{
    public class ZonkGameHub : Hub
    {
        public async Task JoinGame(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("PlayerJoined", Context.ConnectionId);
        }

        public async Task RollDice(string roomId, int[] dice)
        {
            await Clients.Group(roomId).SendAsync("DiceRolled", Context.ConnectionId, dice);
        }

        public async Task EndTurn(string roomId, int score)
        {
            await Clients.Group(roomId).SendAsync("TurnEnded", Context.ConnectionId, score);
        }
    }
}