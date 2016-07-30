using Bot.Models.Contracts;

namespace Bot.Client.Contracts {
  public interface ISender {
    void Send(IPublicMessage publicMessage);
    void Send(IMute mute);
    void Send(IUnMuteBan unMuteBan);
    void Send(ISubonly subonly);
    void Send(IBan ban);
    void Send(IBroadcast broadcast);
  }
}
