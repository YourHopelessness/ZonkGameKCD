using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using System.Threading.Channels;
using ZonkGameCore.Utils;

namespace ZonkGameAI.RPC
{
    public interface IGrpcChannelSingletone
    {
        /// <summary>
        /// Получить канал для подключения к GRC сервису
        /// </summary>
        /// <returns>Канал для подключения</returns>
        GrpcChannel GetChannel();
    }

    /// <summary>
    /// Класс синглтон для хранеиня подключения
    /// </summary>
    /// <param name="configuration">конфигурация апи</param>
    public class GrpcChannelSingletone(IOptions<GameZonkConfiguration> configuration) : IDisposable, IGrpcChannelSingletone
    {
        private readonly GrpcChannel? _channel;
        private readonly string _address = configuration.Value?.AIChannelAdress 
            ?? throw new ArgumentNullException("Адрес сервера Grpc не задан");

        public GrpcChannel GetChannel()
        {
            return _channel ?? GrpcChannel.ForAddress(_address);
        }

        public void Dispose() => _channel?.Dispose();
    }
}
