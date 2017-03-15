using Bot.Database.Entities;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class UserRepository : BaseRepository<UserEntity>, IUserRepository {
    public UserRepository(DbSet<UserEntity> entities) : base(entities) { }

  }
}
