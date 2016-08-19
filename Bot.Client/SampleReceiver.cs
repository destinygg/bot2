using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Client.Contracts;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client {
  public class SampleReceiver : IReceiver {
    private readonly IList<IPublicMessageReceived> _context;
    private readonly IEnumerable<IReceived> _received;
    private readonly ISender _sender;

    public SampleReceiver(IEnumerable<IReceived> received, ISender sender) {
      _received = received;
      _sender = sender;
      _context = new List<IPublicMessageReceived>();
    }
    
    public void Run(IReceivedProcessor receivedProcessor) {
      foreach (var received in _received) {
        if (received is IPublicMessageReceived) {
          var publicMessageReceived = (IPublicMessageReceived) received;
          _context.Add(publicMessageReceived);
          Console.WriteLine($"Public message received: {publicMessageReceived.Text}");
          var outbox = receivedProcessor.Process(publicMessageReceived, _context);
          outbox.ContinueWith(_Send);
        }
      }
    }

    private void _Send(Task<IEnumerable<ISendable>> taskOutbox) {
      foreach (var sendable in taskOutbox.Result) {
        _sender.Send(sendable);
      }
    }
    
  }
}
