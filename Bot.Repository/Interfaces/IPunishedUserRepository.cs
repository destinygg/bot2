using Bot.Models;

namespace Bot.Repository.Interfaces {
  public interface IPunishedUserRepository {
    PunishedUser GetUser(string nick);
    void Increment(string nick, string term);
  }
}
