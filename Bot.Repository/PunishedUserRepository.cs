using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bot.Database.Entities;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class PunishedUserRepository : IPunishedUserRepository {

    private readonly DbSet<PunishedUserEntity> _entities;

    public PunishedUserRepository(DbSet<PunishedUserEntity> entities) {
      _entities = entities;
    }

    public PunishedUserEntity SingleOrDefault(Expression<Func<PunishedUserEntity, bool>> predicate) => _entities
      .Include(x => x.AutoPunishmentEntity)
      .SingleOrDefault(predicate);

    public IEnumerable<PunishedUserEntity> GetAllWithIncludes() => _entities
      .Include(x => x.AutoPunishmentEntity)
      .ToList();
  }
}
