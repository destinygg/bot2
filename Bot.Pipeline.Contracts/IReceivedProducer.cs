using Bot.Logic.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface IReceivedProducer {
    void Run(IReceivedProcessor receivedProcessor);
  }
}
