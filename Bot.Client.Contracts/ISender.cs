using Bot.Models.Contracts;

namespace Bot.Client.Contracts {
  public interface ISender {
    void Send(IPublicMessageReceived publicMessageReceived);
    void Send(IMuteReceived muteReceived);
    void Send(IUnMuteBanReceived unMuteBanReceived);
    void Send(ISubonlyReceived subonlyReceived);
    void Send(IBanReceived banReceived);
    void Send(IBroadcastReceived broadcastReceived);
  }
}
