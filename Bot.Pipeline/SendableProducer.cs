using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class SendableProducer : ISendableProducer {
    private readonly IContextualizedProducer _contextualizedProducer;
    private readonly IContextualizedProcessor _contextualizedProcessor;

    public SendableProducer(IContextualizedProducer contextualizedProducer, IContextualizedProcessor contextualizedProcessor) {
      _contextualizedProducer = contextualizedProducer;
      _contextualizedProcessor = contextualizedProcessor;
    }

    public ISourceBlock<IReadOnlyList<ISendable>> SendableBlock {
      get {
        var transform = new TransformBlock<IContextualized, IReadOnlyList<ISendable>>(r => Transform(r), new ExecutionDataflowBlockOptions {
          MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded,
          EnsureOrdered = false
        });
        _contextualizedProducer.ContextualizedBlock.LinkTo(transform);
        return transform;
      }
    }
    private IReadOnlyList<ISendable> Transform(IContextualized contextualized) {
      return _contextualizedProcessor.Process(contextualized).ToList();
    }
  }
}
