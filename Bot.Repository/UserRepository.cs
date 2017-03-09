using Bot.Database.Entities;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class UserRepository : BaseRepository<User>, IUserRepository {
    public UserRepository(DbSet<User> entities) : base(entities) { }

  }
}
