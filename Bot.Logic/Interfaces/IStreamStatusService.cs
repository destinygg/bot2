using Bot.Repository.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IStreamStatusService {
    StreamStatus Refresh();
  }
}
