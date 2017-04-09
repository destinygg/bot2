using Bot.Database.Entities;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class AutoPunishmentRepository : BaseRepository<AutoPunishmentEntity>, IAutoPunishmentRepository {
    public AutoPunishmentRepository(DbSet<AutoPunishmentEntity> entities) : base(entities) { }

  }
}
