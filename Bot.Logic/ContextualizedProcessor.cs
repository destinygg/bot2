using System;
using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class ContextualizedProcessor : IContextualizedProcessor {
    private readonly IBanGenerator _banScanner;
    private readonly IModCommandGenerator _modCommandGenerator;
    private readonly ICommandGenerator _commandScanner;

    public ContextualizedProcessor(IBanGenerator banScanner, ICommandGenerator commandScanner, IModCommandGenerator modCommandGenerator) {
      _banScanner = banScanner;
      _modCommandGenerator = modCommandGenerator;
      _commandScanner = commandScanner;
    }

    public IReadOnlyList<ISendable> Process(IContextualized contextualized) {
      var outbox = new List<ISendable>();
      var message = contextualized.First as IMessageReceived;
      if (message != null) {
        Console.WriteLine(message.Text);
        if (message.FromMod) {
          outbox.AddRange(_modCommandGenerator.Scan(contextualized));
          outbox.AddRange(_commandScanner.Scan(contextualized));
        } else if (message is IPublicMessageReceived) {
          outbox.AddRange(_banScanner.Scan(contextualized));
          if (outbox.Count == 0) {
            outbox.AddRange(_commandScanner.Scan(contextualized));
          }
        }
      }
      return outbox;
    }

  }
}
