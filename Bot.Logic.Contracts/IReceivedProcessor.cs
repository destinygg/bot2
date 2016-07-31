using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IReceivedProcessor {
    void Process(IPrivateMessageReceived privateMessageReceived);
    void Process(IPublicMessageReceived publicMessageReceived);
    void Process(IMuteReceived muteReceived);
    void Process(IUnMuteBanReceived unMuteBanReceived);
    void Process(ISubonlyReceived subonlyReceived);
    void Process(IBanReceived banReceived);
    void Process(IBroadcastReceived broadcastReceived);
  }
}
