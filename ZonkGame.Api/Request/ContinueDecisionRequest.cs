namespace ZonkGameApi.Request
{
    public class ContinueDecisionRequest
    {
        public Guid GameId { get; set; }
        public bool ShouldContinue { get; set; }
        public Guid PlayerId { get; set; }
    }
}
