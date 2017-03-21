using Bot.Database.Entities;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class AutoPunishmentRepository : IAutoPunishmentRepository {

    private readonly DbSet<AutoPunishmentEntity> _entities;

    public AutoPunishmentRepository(DbSet<AutoPunishmentEntity> entities) {
      _entities = entities;
    }

  }
}
