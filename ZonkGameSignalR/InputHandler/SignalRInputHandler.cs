using ZonkGameCore.InputParams;

namespace ZonkGameSignalR.InputHandler
{
    public class SignalRInputHandler : IInputAsyncHandler
    {
        private object connectionId;

        public SignalRInputHandler(object connectionId)
        {
            this.connectionId = connectionId;
        }

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
