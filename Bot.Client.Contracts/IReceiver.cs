using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client.Contracts {
  public interface IReceiver {
    void Run(IReceivedProcessor receivedProcessor);
  }
}
