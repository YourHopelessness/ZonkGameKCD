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

        public async Task<IEnumerable<int>> HandleSelectDiceInputAsync(IEnumerable<int> roll)
        {
            var response = await _serviceClient.GetSelectedDicesAsync(
                new SelectedDicesRequest { Dices = { roll } });

            return [..response.Dices.Select(x => x)];
        }

        public async Task<bool> HandleShouldContinueGameInputAsync()
        {
            var response = await _serviceClient.GetContinuationDecisionAsync(
                new ContinuationDecisionRequest());

            return response.ContinueGame;
        }
    }
}
