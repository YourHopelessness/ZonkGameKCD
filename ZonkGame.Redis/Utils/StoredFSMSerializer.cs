using System.Text.Json;
using ZonkGameCore.Model;

namespace ZonkGameRedis.Utils
{
    /// <summary>
    /// The serializer of the state of the game
    /// </summary>
    public static class StoredFSMSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Serializes the provided state to JSON.
        /// </summary>
        /// <param name="state">State to serialize</param>
        /// <returns>JSON representation</returns>
        public static string Serialize(StoredFSMModel state)
        {
            return JsonSerializer.Serialize(state, Options);
        }

        /// <summary>
        /// Deserializes JSON into a <see cref="StoredFSMModel"/>.
        /// </summary>
        /// <param name="json">Serialized game state</param>
        /// <returns>Deserialized model</returns>
        public static StoredFSMModel Deserialize(string json)
        {
            return JsonSerializer.Deserialize<StoredFSMModel>(json, Options)!;
        }
    }
}
