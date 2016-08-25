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

    public ISourceBlock<ISendable> Produce {
      get {
        var transform = new TransformBlock<IContextualized, ISendable>(r => Transform(r));
        _sourceBlock.LinkTo(transform);
        return transform;
      }
    }
    private ISendable Transform(IContextualized contextualized) {
      return new PublicMessage("hi, processing should occur here");
    }
  }
}
