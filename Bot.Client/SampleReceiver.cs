using System;
using System.Collections.Generic;
using Bot.Client.Contracts;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client {
  public class SampleReceiver : IReceiver {
    private readonly IEnumerable<IReceived> _received;

    public SampleReceiver(IEnumerable<IReceived> received) {
      _received = received;
    }

    public void Receive(IPrivateMessageReceived privateMessageReceived) {
      Print(privateMessageReceived);
    }

    public void Receive(IPublicMessageReceived publicMessageReceived) {
      Print(publicMessageReceived);
    }

    public void Receive(IMuteReceived muteReceived) {
      Print(muteReceived);
    }

    public void Receive(IUnMuteBanReceived unMuteBanReceived) {
      Print(unMuteBanReceived);
    }

    public void Receive(ISubonlyReceived subonlyReceived) {
      Print(subonlyReceived);
    }

    public void Receive(IBanReceived banReceived) {
      Print(banReceived);
    }

    public void Receive(IBroadcastReceived broadcastReceived) {
      Print(broadcastReceived);
    }

    public void Run(IReceivedProcessor receivedProcessor) {
      foreach (var received in _received) {
        if (received is IPrivateMessageReceived) {

        }
      }
    }

    private void Print(IReceived received) {
      Console.WriteLine(received.ToString());
    }
  }
}
