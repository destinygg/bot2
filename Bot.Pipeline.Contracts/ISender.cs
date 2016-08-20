using Bot.Models.Contracts;

namespace Bot.Client.Contracts {
  public interface ISender {
    void Send(ISendable sendable);
  }
}
