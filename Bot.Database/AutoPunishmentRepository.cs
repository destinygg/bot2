using Bot.Database.Entities;
using Bot.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class AutoPunishmentRepository : BaseRepository<AutoPunishment>, IAutoPunishmentRepository {
    public AutoPunishmentRepository(DbSet<AutoPunishment> entities) : base(entities) { }

  }
}
