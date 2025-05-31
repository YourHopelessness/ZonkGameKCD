using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.Json;
using ZonkGame.DB.Entites.Zonk;

namespace ZonkGame.DB.Context
{
    public class ZonkDbContext(DbContextOptions<ZonkDbContext> options) : DbContext(options)
    {
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<GamePlayer> GamePlayers { get; set; } = null!;
        public DbSet<GameAudit> GameAudits { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Game properties
            modelBuilder.Entity<Game>()
               .HasMany(g => g.GamePlayers)
               .WithOne(g => g.Game)
               .HasForeignKey("game_id")
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game>()
                .Property(g => g.CreatedAt)
                .HasConversion(typeof(DateTime))
                .HasDefaultValueSql("timezone('utc', now())");

            modelBuilder.Entity<Game>()
                .Property(g => g.EndedAt)
                .HasConversion(typeof(DateTime?));

            modelBuilder.Entity<Game>()
                .Property(g => g.GameType)
                .HasConversion<string>();

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Winner)
                .WithMany()
                .HasForeignKey("winner_id");
            #endregion

            #region Player properties
            modelBuilder.Entity<Player>()
                .Property(g => g.PlayerType)
                .HasConversion<string>();

            modelBuilder.Entity<Player>()
               .HasMany(g => g.GamePlayers)
               .WithOne(g => g.Player)
               .HasForeignKey("player_id")
               .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region GameAudit properties
            modelBuilder.Entity<GameAudit>()
                .Property(g => g.EventType)
                .HasConversion<string>();

            modelBuilder.Entity<GameAudit>()
                .Property(g => g.CurrentRoll)
                .HasConversion(
                    v => v != null ? v.ToArray() : null,
                    v => v != null ? v.AsEnumerable() : null);

            modelBuilder.Entity<GameAudit>()
               .Property(g => g.CurrentRoll)
               .HasConversion(
                    v => v != null ? v.ToArray() : null,
                    v => v != null ? v.AsEnumerable() : null);

            var jaggedConverter = new ValueConverter<IEnumerable<int[]>?, string?>(
                v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => v == null ? null : JsonSerializer.Deserialize<IEnumerable<int[]>>(v, (JsonSerializerOptions?)null)!);
            modelBuilder.Entity<GameAudit>()
               .Property(g => g.AvaliableCombination)
               .HasConversion(jaggedConverter);

            modelBuilder.Entity<GameAudit>()
               .Property(g => g.RecordTime)
               .HasConversion<DateTime>()
               .HasDefaultValueSql("timezone('utc', now())");

            modelBuilder.Entity<GameAudit>()
                .HasOne(g => g.Game)
                .WithMany(g => g.GameAudit);

            modelBuilder.Entity<GameAudit>()
                .HasOne(g => g.CurrentPlayer)
                .WithMany(g => g.GameAudit);
            #endregion
        }
    }
}
