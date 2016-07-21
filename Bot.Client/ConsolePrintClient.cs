using System;
using Bot.Client.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client {
  public class ConsolePrintClient : IClientVisitor {
    public virtual void Visit(IPrivateMessageReceived privateMessageReceived) {
      Print(privateMessageReceived);
    }

    public virtual void Visit(IPublicMessageReceived publicMessageReceived) {
      Print(publicMessageReceived);
    }

    public virtual void Visit(IMuteReceived muteReceived) {
      Print(muteReceived);
    }

    public virtual void Visit(IUnMuteBanReceived unMuteBanReceived) {
      Print(unMuteBanReceived);
    }

    public virtual void Visit(ISubonlyReceived subonlyReceived) {
      Print(subonlyReceived);
    }

    public virtual void Visit(IBanReceived banReceived) {
      Print(banReceived);
    }

    public virtual void Visit(IBroadcastReceived broadcastReceived) {
      Print(broadcastReceived);
    }

    private void Print(IReceived received) {
      Console.WriteLine(received.ToString());
    }
  }
}
