using Bot.Database.Entities;
using Bot.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class UserRepository : BaseRepository<User>, IUserRepository {
    public UserRepository(DbSet<User> entities) : base(entities) { }

  }
}
