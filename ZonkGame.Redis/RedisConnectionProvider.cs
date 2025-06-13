using Microsoft.Extensions.Options;
using StackExchange.Redis;
using ZonkGameCore.ApiConfiguration;

namespace ZonkGameRedis
{
    public interface IRedisConnectionProvider : IDisposable
    {
        /// <summary>
        /// Provides access to a Redis database.
        /// </summary>
        /// <returns>Redis database instance</returns>
        IDatabase GetDatabase();
    }

    public class RedisConnectionProvider : IRedisConnectionProvider
    {
        private readonly ConnectionMultiplexer _multiplexer;
        private readonly IDatabase _db;

        public RedisConnectionProvider(IOptions<GameZonkConfiguration> options)
        {
            var connString = options.Value.RedisConnectionString
                             ?? throw new ArgumentNullException(nameof(options));
            _multiplexer = ConnectionMultiplexer.Connect(connString);
            _db = _multiplexer.GetDatabase();
        }

        /// <inheritdoc />
        public IDatabase GetDatabase() => _db;

        /// <inheritdoc />
        public void Dispose() => _multiplexer.Dispose();
    }

}
