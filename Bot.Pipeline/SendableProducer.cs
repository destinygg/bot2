using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class SendableProducer : ISendableProducer {
    private readonly ISourceBlock<IContextualized> _sourceBlock;

    public SendableProducer(ISourceBlock<IContextualized> sourceBlock) {
      _sourceBlock = sourceBlock;
    }

    public ISourceBlock<IReadOnlyList<ISendable>> Produce {
      get {
        var transform = new TransformBlock<IContextualized, IReadOnlyList<ISendable>>(r => Transform(r));
        _sourceBlock.LinkTo(transform);
        return transform;
      }
    }
    private IReadOnlyList<ISendable> Transform(IContextualized contextualized) {
      return new List<ISendable> { new PublicMessage("hi, processing should occur here") };
    }
  }
}
