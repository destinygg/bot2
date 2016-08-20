using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface ISender {
    void Send(ISendable sendable);
  }
}
