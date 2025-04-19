using Grpc.Net.Client;
using ZonkGameCore.InputParams;

namespace ZonkGameAI.RPC.AIClient
{
    public class ZonkAIClient(GrpcChannel channel) : IInputAsyncHandler
    {
        private readonly GrpcChannel _channel = channel;

        public Task<IEnumerable<int>> HandleSelectDiceInputAsync(IEnumerable<int> roll)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HandleShouldContinueGameInputAsync()
        {
            throw new NotImplementedException();
        }
    }
}
