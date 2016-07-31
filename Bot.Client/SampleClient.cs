using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client {
  public class SampleClient {
    private readonly IEnumerable<IReceived> _received;
    private readonly IReceivedProcessor _receivedProcessor;

    public SampleClient(IEnumerable<IReceived> received, IReceivedProcessor receivedProcessor) {
      _received = received;
      _receivedProcessor = receivedProcessor;
      var receiver = new SampleReceiver(_received);
    }

    public void Run() {
      foreach (var received in _received) {
        if (received is IPublicMessageReceived)
          _receivedProcessor.Process(received as IPublicMessageReceived);
      }
    }
  }
}
