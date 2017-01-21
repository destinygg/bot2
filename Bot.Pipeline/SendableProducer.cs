using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class SendableProducer : ISendableProducer {

    public SendableProducer(IContextualizedProducer contextualizedProducer, ISendableGenerator sendableGenerator) {
      SendableBlock = new TransformBlock<IContextualized, IReadOnlyList<ISendable>>(c => Transform(sendableGenerator, c), new ExecutionDataflowBlockOptions {
        MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded,
        EnsureOrdered = false
      });
      contextualizedProducer.ContextualizedBlock.LinkTo(SendableBlock);
    }

    public TransformBlock<IContextualized, IReadOnlyList<ISendable>> SendableBlock { get; }

    private IReadOnlyList<ISendable> Transform(ISendableGenerator sendableGenerator, IContextualized contextualized) => sendableGenerator.Generate(contextualized);
  }
}
