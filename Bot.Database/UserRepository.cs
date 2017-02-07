using Bot.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class UserRepository : BaseRepository<User> {
    public UserRepository(DbSet<User> entities) : base(entities) { }

  }
}
