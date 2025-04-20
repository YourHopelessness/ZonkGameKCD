namespace ZonkGameApi.Request
{
    public class DiceSelectionRequest
    {
        public Guid GameId { get; set; }
        public List<int> SelectedDice { get; set; }
    }
}
