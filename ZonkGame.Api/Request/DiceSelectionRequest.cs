namespace ZonkGameApi.Request
{
    public class DiceSelectionRequest
    {
        /// <summary> Игра </summary>
        public Guid GameId { get; set; }

        /// <summary> Игрок </summary>
        public Guid PlayerId { get; set; }

        /// <summary> Отложенные кости </summary>
        public List<int> SelectedDice { get; set; }
    }
}
