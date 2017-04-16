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

    public void Add(AutoPunishment autoPunishment) {
      var entity = new AutoPunishmentEntity();
      autoPunishment.CopyTo(entity);
      _entities.Add(entity);
    }

    public void Update(AutoPunishment autoPunishment) {
      var entity = _single(autoPunishment.Id);
      autoPunishment.CopyTo(entity);
      _entities.Update(entity);
    }

    private AutoPunishmentEntity _single(int id) =>
      _entities.Include(x => x.PunishedUsers).Single(x => x.Id == id);

    public IList<AutoPunishment> GetAllWithUser =>
      _entities.Include(e => e.PunishedUsers).ToList().Select(e => new AutoPunishment(e)).ToList();
  }
}
