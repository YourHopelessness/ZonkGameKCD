using System.Text.Json;
using ZonkGameCore.Model;

namespace ZonkGameRedis.Utils
{
    /// <summary>
    /// Сериализатор состояния игры
    /// </summary>
    public static class StoredFSMSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static string Serialize(StoredFSMModel state)
        {
            return JsonSerializer.Serialize(state, Options);
        }

        public static StoredFSMModel Deserialize(string json)
        {
            return JsonSerializer.Deserialize<StoredFSMModel>(json, Options)!;
        }
    }
}
