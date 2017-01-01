using System;
using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class ContextualizedProcessor : IContextualizedProcessor {
    private readonly IBanGenerator _banGenerator;
    private readonly IModCommandGenerator _modCommandGenerator;
    private readonly ICommandGenerator _commandGenerator;

    public ContextualizedProcessor(IBanGenerator banGenerator, ICommandGenerator commandGenerator, IModCommandGenerator modCommandGenerator) {
      _banGenerator = banGenerator;
      _modCommandGenerator = modCommandGenerator;
      _commandGenerator = commandGenerator;
    }

    public IReadOnlyList<ISendable> Process(IContextualized contextualized) {
      var outbox = new List<ISendable>();
      var message = contextualized.First as IMessageReceived;
      if (message != null) {
        Console.WriteLine(message.Text);
        if (message.FromMod) {
          outbox.AddRange(_modCommandGenerator.Scan(contextualized));
          outbox.AddRange(_commandGenerator.Scan(contextualized));
        } else if (message is IPublicMessageReceived) {
          outbox.AddRange(_banGenerator.Scan(contextualized));
          if (outbox.Count == 0) {
            outbox.AddRange(_commandGenerator.Scan(contextualized));
          }
        }
      }
      return outbox;
    }

  }
}
