using System.Collections.Generic;
using System.Linq;
using Bot.Database.Entities;
using Bot.Models;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class AutoPunishmentRepository : IAutoPunishmentRepository {

    private readonly DbSet<AutoPunishmentEntity> _entities;

    public AutoPunishmentRepository(DbSet<AutoPunishmentEntity> entities) {
      _entities = entities;
    }

    public void Add(AutoPunishment autoPunishment) =>
      _entities.Add(autoPunishment.ToEntity());

    public IEnumerable<AutoPunishment> GetAll() =>
      _entities.Include(x => x.PunishedUsers).ToList().Select(x => new AutoPunishment(x));
  }
}
