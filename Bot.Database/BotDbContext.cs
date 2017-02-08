using Bot.Database.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class BotDbContext : DbContext {

    #region DbSet
    public DbSet<StateInteger> StateIntegers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AutoPunishment> AutoPunishments { get; set; }
    public DbSet<PunishedUser> PunishedUsers { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<StateInteger>().HasKey(si => si.Key);
      modelBuilder.Entity<PunishedUser>()
        .HasKey(pu => new { pu.UserId, pu.AutoPunishmentId });

      //EFCore doesn't support many to many yet.
      modelBuilder.Entity<PunishedUser>()
          .HasOne(pt => pt.User)
          .WithMany(p => p.PunishedUsers)
          .HasForeignKey(pt => pt.UserId);

      modelBuilder.Entity<PunishedUser>()
          .HasOne(pt => pt.AutoPunishment)
          .WithMany(t => t.PunishedUsers)
          .HasForeignKey(pt => pt.AutoPunishmentId);

      base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      var sqliteConn = new SqliteConnection(@"DataSource = Bot.db");
      optionsBuilder.UseSqlite(sqliteConn);
    }

  }
}
