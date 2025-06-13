using Grpc.Core;
using Grpc.Net.Client;
using Zonk;
using ZonkGameCore.InputParams;

namespace ZonkGameAI.RPC.AIClient
{
    public class GrpcAgentInputHandler(GrpcChannel channel) : IInputAsyncHandler
    {
        private readonly ZonkService.ZonkServiceClient _serviceClient =
            new(channel);

        /// <summary>
        /// Requests selected dice from the gRPC agent.
        /// </summary>
        /// <param name="roll">Current roll values</param>
        /// <param name="gameid">Game identifier</param>
        /// <param name="playerId">Player identifier</param>
        public async Task<IEnumerable<int>?> HandleSelectDiceInputAsync(IEnumerable<int> roll, Guid gameid, Guid playerId)
        {
            var response = await _serviceClient.GetSelectedDicesAsync(
                new SelectedDicesRequest { GameId = gameid.ToString(), Dices = { roll } });

            return [..response.Dices.Select(x => x)];
        }

        /// <summary>
        /// Requests continuation decision from the gRPC agent.
        /// </summary>
        /// <param name="gameid">Game identifier</param>
        /// <param name="playerId">Player identifier</param>
        /// <returns>Whether the agent wants to continue the game</returns>
        public async Task<bool?> HandleShouldContinueGameInputAsync(Guid gameid, Guid playerId)
        {
            var response = await _serviceClient.GetContinuationDecisionAsync(
                new ContinuationDecisionRequest() { GameId = gameid.ToString()});

            return response.ContinueGame;
        }
    }
}
