using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;
using Bot.Tools;

namespace Bot.Pipeline {
  public class ContextualizedProducer : IContextualizedProducer {
    private readonly IReceivedProducer _receivedProducer;
    private readonly List<IReceived> _allReceived = new List<IReceived>(); // Todo: Optimization: Use a circular buffer

    public ContextualizedProducer(IReceivedProducer receivedProducer) {
      _receivedProducer = receivedProducer;
    }

    public ISourceBlock<IContextualized> ContextualizedBlock {
      get {
        var transform = new TransformBlock<IReceived, IContextualized>(r => Transform(r));
        _receivedProducer.ReceivedBlock.LinkTo(transform);
        return transform;
      }
    }

    private IContextualized Transform(IReceived first) {
      _allReceived.Insert(0, first);
      if (_allReceived.Count > Settings.ContextSize) {
        _allReceived.RemoveAt(Settings.ContextSize);
      }
      return new Contextualized(_allReceived.ToList()); // The .ToList() creates a new instance; important!
    }
  }
}
