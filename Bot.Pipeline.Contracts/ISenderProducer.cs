using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface ISenderProducer {
    void Send(ISendable sendable);
  }
}
