using System;
using Bot.Client.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client {
  public abstract class ConsolePrintClient : IReceiver {
    public virtual void Receive(IPrivateMessageReceived privateMessageReceived) {
      Print(privateMessageReceived);
    }

    public virtual void Receive(IPublicMessageReceived publicMessageReceived) {
      Print(publicMessageReceived);
    }

    public virtual void Receive(IMuteReceived muteReceived) {
      Print(muteReceived);
    }

    public virtual void Receive(IUnMuteBanReceived unMuteBanReceived) {
      Print(unMuteBanReceived);
    }

    public virtual void Receive(ISubonlyReceived subonlyReceived) {
      Print(subonlyReceived);
    }

    public virtual void Receive(IBanReceived banReceived) {
      Print(banReceived);
    }

    public virtual void Receive(IBroadcastReceived broadcastReceived) {
      Print(broadcastReceived);
    }

    public virtual void Run() {
      throw new NotImplementedException();
    }

    private void Print(IReceived received) {
      Console.WriteLine(received.ToString());
    }
  }
}
