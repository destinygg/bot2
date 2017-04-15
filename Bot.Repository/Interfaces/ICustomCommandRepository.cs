using System.Collections.Generic;
using Bot.Models;

namespace Bot.Repository.Interfaces {
  public interface ICustomCommandRepository {
    IList<CustomCommand> GetAll { get; }
    void Add(string command, string response);
    void Delete(string command);
  }
}
