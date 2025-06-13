namespace ZonkGameApi.Request
{
    public class DiceSelectionRequest
    {
        /// <summary>Game</summary>
        public Guid GameId { get; set; }

        /// <summary>Player</summary>
        public Guid PlayerId { get; set; }

        /// <summary>Delayed bones</summary>
        public List<int> SelectedDice { get; set; }
    }
}
