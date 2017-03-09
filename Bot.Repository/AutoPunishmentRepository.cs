using Bot.Database;
using Bot.Database.Entities;
using Bot.Database.Interfaces;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class AutoPunishmentRepository : BaseRepository<AutoPunishment>, IAutoPunishmentRepository {
    public AutoPunishmentRepository(DbSet<AutoPunishment> entities) : base(entities) { }

  }
}
