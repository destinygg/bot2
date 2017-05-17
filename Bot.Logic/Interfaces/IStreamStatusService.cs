using Bot.Models;

namespace Bot.Logic.Interfaces {
  public interface IStreamStatusService {
    StreamState Get();
  }
}
