using System.Collections.Generic;
using System.Linq;
using Bot.Database.Entities;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class PeriodicMessageRepository : IPeriodicMessageRepository {

    private readonly DbSet<PeriodicMessageEntity> _entities;

    public PeriodicMessageRepository(DbSet<PeriodicMessageEntity> entities) {
      _entities = entities;
    }

    public string Get(string message) => _entities.Where(x => x.Message == message).Select(c => c.Message).SingleOrDefault();

    public IList<string> GetAll => _entities.Select(c => c.Message).ToList();

    public void Add(string message) => _entities.Add(new PeriodicMessageEntity(message));

    public void Update(string message) {
      var commandToUpdate = _entities.Single(x => x.Message == message);
      commandToUpdate.Message = message;
      _entities.Update(commandToUpdate);
    }

    public void Delete(string message) => _entities.Remove(_entities.Single(x => x.Message == message));

  }
}
