namespace ZonkGameRedis.Entities
{
    public class Leaderboard
    {
        public string PlayerName { get; set; } = string.Empty;

        public int TotalScore { get; set; } = 0;

        public Guid PlayerId { get; set; } = Guid.NewGuid();

        public int Wins { get; set; } = 0;

        public int Losses { get; set; } = 0;    
    }
}
