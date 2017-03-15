using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bot.Database.Entities;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class PunishedUserRepository : BaseRepository<PunishedUserEntity>, IPunishedUserRepository {
    public PunishedUserRepository(DbSet<PunishedUserEntity> entities) : base(entities) { }

    public override PunishedUserEntity SingleOrDefault(Expression<Func<PunishedUserEntity, bool>> predicate) => Entities
      .Include(x => x.UserEntity)
      .Include(x => x.AutoPunishmentEntity)
      .SingleOrDefault(predicate);

    public IEnumerable<PunishedUserEntity> GetAllWithIncludes() => Entities
      .Include(x => x.UserEntity)
      .Include(x => x.AutoPunishmentEntity)
      .ToList();
  }
}
