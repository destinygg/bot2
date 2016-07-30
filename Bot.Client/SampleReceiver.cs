using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client {
  public class SampleReceiver : ConsolePrintReceiver {
    private readonly IEnumerable<IReceived> _received;
    private readonly IProcessReceived _processReceived;

    public SampleReceiver(IEnumerable<IReceived> received, IProcessReceived processReceived) {
      _received = received;
      _processReceived = processReceived;
    }

    public override void Run() {
      foreach (var received in _received) {
        if (received is IPublicMessageReceived)
          _processReceived.Process(received as IPublicMessageReceived);
      }
    }
  }
}
