using System.Collections.Generic;

namespace Bot.Repository.Interfaces {
  public interface IPeriodicMessageRepository {
    string Get(string message);
    IList<string> GetAll { get; }
    void Add(string message);
    void Update(string message);
    void Delete(string message);
  }
}
