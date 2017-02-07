using Bot.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class AutoPunishmentRepository : BaseRepository<AutoPunishment> {
    public AutoPunishmentRepository(DbSet<AutoPunishment> entities) : base(entities) { }

  }
}
