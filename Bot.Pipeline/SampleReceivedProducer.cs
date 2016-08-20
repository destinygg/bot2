using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;
using Bot.Tools;

namespace Bot.Pipeline {
  public class SampleReceivedProducer : IReceivedProducer {
    private readonly LinkedList<IPublicMessageReceived> _context;
    private readonly IEnumerable<IReceived> _received;
    private readonly ISenderProducer _senderProducer;

    public SampleReceivedProducer(IEnumerable<IReceived> received, ISenderProducer senderProducer) {
      _received = received;
      _senderProducer = senderProducer;
      _context = new LinkedList<IPublicMessageReceived>();
      for (var i = 0; i < Settings.ContextSize; i++) {
        _context.AddFirst(new PublicMessageReceived(true));
      }
    }

    public void Run(IReceivedProcessor receivedProcessor) {
      foreach (var received in _received) {
        if (received is IPublicMessageReceived) {
          var publicMessageReceived = (IPublicMessageReceived) received;
          _context.AddFirst(publicMessageReceived);
          _context.RemoveLast();
          Console.WriteLine($"Public message received: {publicMessageReceived.Text}");
          var outbox = receivedProcessor.Process(publicMessageReceived, new List<IPublicMessageReceived>(_context)); // IPublicMessageReceived needs to be strongly readonly to prevent side effects and remain thread-safe
          outbox.ContinueWith(_Send);
        }
      }
    }

    private void _Send(Task<IEnumerable<ISendable>> taskOutbox) {
      foreach (var sendable in taskOutbox.Result) {
        _senderProducer.Send(sendable);
      }
    }

  }
}
