using System.Collections.Generic;
using Bot.Models;

namespace Bot.Repository.Interfaces {
  public interface IInMemoryRepository {
    ICollection<Nuke> Nukes { get; }
    void Add(Nuke nuke);
    void Remove(Nuke nuke);
  }
}
