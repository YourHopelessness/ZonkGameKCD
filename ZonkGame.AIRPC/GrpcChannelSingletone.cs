using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using System.Threading.Channels;
using ZonkGameCore.ApiConfiguration;

namespace ZonkGameAI.RPC
{
    public interface IGrpcChannelSingletone
    {
        /// <summary>
        /// Get a channel for connecting to a GRC service
        /// </summary>
        /// <returns>Channel for connection</returns>
        GrpcChannel GetChannel();
    }

    /// <summary>
    /// Single -line
    /// </summary>
    /// <param name="configuration">API configuration</param>
    public class GrpcChannelSingletone(IOptions<GameZonkConfiguration> configuration) : IDisposable, IGrpcChannelSingletone
    {
        private readonly GrpcChannel? _channel;
        private readonly string _address = configuration.Value?.AIChannelAdress 
            ?? throw new ArgumentNullException("GRPC server address is not set");

        public GrpcChannel GetChannel()
        {
            return _channel ?? GrpcChannel.ForAddress(_address);
        }

        public void Dispose() => _channel?.Dispose();
    }
}
