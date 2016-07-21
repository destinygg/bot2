using Bot.Models.Contracts;

namespace Bot.Client.Contracts {
  public interface IClientVisitor {
    void Visit(IPrivateMessageReceived privateMessageReceived);
    void Visit(IPublicMessageReceived publicMessageReceived);
    void Visit(IMuteReceived muteReceived);
    void Visit(IUnMuteBanReceived unMuteBanReceived);
    void Visit(ISubonlyReceived subonlyReceived);
    void Visit(IBanReceived banReceived);
    void Visit(IBroadcastReceived broadcastReceived);
  }
}
