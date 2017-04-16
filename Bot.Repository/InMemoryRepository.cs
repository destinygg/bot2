using System.Collections.Generic;
using Bot.Models;
using Bot.Repository.Interfaces;

namespace Bot.Repository {
  public class InMemoryRepository : IInMemoryRepository {
    public ICollection<Nuke> Nukes { get; } = new List<Nuke>();
    public void Add(Nuke nuke) => Nukes.Add(nuke);
    public void Remove(Nuke nuke) => Nukes.Remove(nuke);
  }
}
