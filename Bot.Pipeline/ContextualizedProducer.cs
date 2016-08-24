using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class ContextualizedProducer : IContextualizedProducer {
    private readonly ISourceBlock<IReceived> _sourceBlock;
    private readonly IList<IReceived> _context;

    public ContextualizedProducer(ISourceBlock<IReceived> sourceBlock) {
      _sourceBlock = sourceBlock;
      _context = new List<IReceived>();
    }

    public ISourceBlock<IContextualized> Produce {
      get {
        var transform = new TransformBlock<IReceived, IContextualized>(r => Transform(r));
        _sourceBlock.LinkTo(transform);
        return transform;
      }
    }

    private IContextualized Transform(IReceived first) {
      _context.Add(first);
      return new Contextualized(_context.ToList());
    }
  }
}
