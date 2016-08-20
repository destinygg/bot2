using Bot.Logic.Contracts;

namespace Bot.Client.Contracts {
  public interface IReceiver {
    void Run(IReceivedProcessor receivedProcessor);
  }
}
