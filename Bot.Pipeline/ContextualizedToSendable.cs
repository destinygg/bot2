using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class ContextualizedToSendable : IContextualizedToSendable {
    private readonly ISendableGenerator _sendableGenerator;

    public ContextualizedToSendable(ISendableGenerator sendableGenerator) {
      _sendableGenerator = sendableGenerator;
    }

    public IReadOnlyList<ISendable> GetSendables(IContextualized contextualized) => _sendableGenerator.Generate(contextualized);
  }
}
