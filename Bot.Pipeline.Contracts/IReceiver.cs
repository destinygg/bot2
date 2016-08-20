using Bot.Logic.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface IReceiver {
    void Run(IReceivedProcessor receivedProcessor);
  }
}
