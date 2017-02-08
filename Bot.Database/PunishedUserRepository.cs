using System.Collections.Generic;
using System.Linq;
using Bot.Database.Entities;
using Bot.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class PunishedUserRepository : BaseRepository<PunishedUser>, IPunishedUserRepository {
    public PunishedUserRepository(DbSet<PunishedUser> entities) : base(entities) { }

    public IEnumerable<PunishedUser> GetAllWithIncludes() => Entities
      .Include(x => x.User)
      .Include(x => x.AutoPunishment)
      .ToList();
  }
}
