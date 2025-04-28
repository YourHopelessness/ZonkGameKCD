using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonkGameCore.Utils;

namespace ZonkGameRedis
{
    public interface IRedisConnectionProvider : IDisposable
    {
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

        public IDatabase GetDatabase() => _db;

        public void Dispose() => _multiplexer.Dispose();
    }

}
