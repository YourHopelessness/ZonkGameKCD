using System.Text.Json;
using ZonkGameCore.Dto;

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

        public static string Serialize(StoredFSM state)
        {
            return JsonSerializer.Serialize(state, Options);
        }

        public static StoredFSM Deserialize(string json)
        {
            return JsonSerializer.Deserialize<StoredFSM>(json, Options)!;
        }
    }
}
