using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bot.Database.Entities;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class PunishedUserRepository : BaseRepository<PunishedUser>, IPunishedUserRepository {
    public PunishedUserRepository(DbSet<PunishedUser> entities) : base(entities) { }

    public override PunishedUser SingleOrDefault(Expression<Func<PunishedUser, bool>> predicate) => Entities
      .Include(x => x.User)
      .Include(x => x.AutoPunishment)
      .SingleOrDefault(predicate);

    public IEnumerable<PunishedUser> GetAllWithIncludes() => Entities
      .Include(x => x.User)
      .Include(x => x.AutoPunishment)
      .ToList();
  }
}
