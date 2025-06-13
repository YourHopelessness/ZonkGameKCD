using ZonkGame.DB.Enum;

namespace ZonkGameCore.Utils
{
    public static class GameModeDefiner
    {
        /// <summary>
        /// Determines the game mode by the type of players
        /// </summary>
        public static ModesEnum GetGameMode(IEnumerable<PlayerTypeEnum> playersType)
        {
            ArgumentNullException.ThrowIfNull(playersType);

            var types = playersType.ToList();
            if (types.Count == 0)
                throw new ArgumentException("Need at least two players", nameof(playersType));

            int realCount = types.Count(t => t == PlayerTypeEnum.RealPlayer);
            int total = types.Count;

            if (realCount == total)
                return ModesEnum.PvP;
            if (realCount == 0)
                return ModesEnum.EvE;
            return ModesEnum.PvE;
        }
    }
}
