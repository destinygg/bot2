using Bot.Database.Entities;
using Bot.Database.Interfaces;
using Bot.Tools;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class BotDbContext : DbContext, IBotDbContext {
    private readonly string _sqlitePath;

    public BotDbContext(ISettings settings) {
      _sqlitePath = settings.SqlitePath;
    }

    #region DbSet
    public DbSet<StateIntegerEntity> StateIntegers { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<AutoPunishmentEntity> AutoPunishments { get; set; }
    public DbSet<PunishedUserEntity> PunishedUsers { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<StateIntegerEntity>().HasKey(si => si.Key);
      modelBuilder.Entity<PunishedUserEntity>()
        .HasKey(pu => new { pu.UserId, pu.AutoPunishmentId });

      //EFCore doesn't support many to many yet.
      modelBuilder.Entity<PunishedUserEntity>()
          .HasOne(pt => pt.UserEntity)
          .WithMany(p => p.PunishedUsers)
          .HasForeignKey(pt => pt.UserId);

      modelBuilder.Entity<PunishedUserEntity>()
          .HasOne(pt => pt.AutoPunishmentEntity)
          .WithMany(t => t.PunishedUsers)
          .HasForeignKey(pt => pt.AutoPunishmentId);

      base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      var sqliteConn = new SqliteConnection($"DataSource = {_sqlitePath}");
      optionsBuilder.UseSqlite(sqliteConn);
    }

  }
}
