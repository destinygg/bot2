using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client {
  public class SampleReceiver : ConsolePrintReceiver {
    private readonly IEnumerable<IReceived> _received;
    private readonly IMessageProcessor _messageProcessor;

    public SampleReceiver(IEnumerable<IReceived> received, IMessageProcessor messageProcessor) {
      _received = received;
      _messageProcessor = messageProcessor;
    }

    public override void Run() {
      foreach (var received in _received) {
        if (received is IPublicMessageReceived)
          _messageProcessor.Process(received as IPublicMessageReceived);
      }
    }
  }
}
