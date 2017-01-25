using Bot.Database.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Bot.Api {
  public class BotDbContext : DbContext {

    #region DbSet
    public DbSet<StateInteger> StateIntegers { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<StateInteger>(b => b.HasKey(si => new { si.Key }));

      base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      var sqliteConn = new SqliteConnection(@"DataSource = Bot.db");
      optionsBuilder.UseSqlite(sqliteConn);
    }
  }
}
