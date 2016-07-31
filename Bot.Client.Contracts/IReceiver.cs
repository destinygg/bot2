using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client.Contracts {
  public interface IReceiver {
    void Receive(IPrivateMessageReceived privateMessageReceived);
    void Receive(IPublicMessageReceived publicMessageReceived);
    void Receive(IMuteReceived muteReceived);
    void Receive(IUnMuteBanReceived unMuteBanReceived);
    void Receive(ISubonlyReceived subonlyReceived);
    void Receive(IBanReceived banReceived);
    void Receive(IBroadcastReceived broadcastReceived);
    void Run(IReceivedProcessor receivedProcessor);
  }
}
