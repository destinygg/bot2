using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Logic {
  public class SendableGenerator : ISendableGenerator {
    private readonly IModCommandGenerator _modCommandGenerator;
    private readonly ICommandGenerator _commandGenerator;
    private readonly IBanGenerator _banGenerator;
    private readonly ILogger _logger;

    public SendableGenerator(IBanGenerator banGenerator, ICommandGenerator commandGenerator, IModCommandGenerator modCommandGenerator, ILogger logger) {
      _banGenerator = banGenerator;
      _modCommandGenerator = modCommandGenerator;
      _logger = logger;
      _commandGenerator = commandGenerator;
    }

    public IReadOnlyList<ISendable> Generate(IContextualized contextualized) {
      var outbox = new List<ISendable>();
      var message = contextualized.Latest as IReceivedMessage<IUser>;
      if (message != null) {
        _logger.LogVerbose(message.Text);
        if (message.IsFromMod()) {
          outbox.AddRange(_modCommandGenerator.Generate(contextualized));
          outbox.AddRange(_commandGenerator.Generate(contextualized));
        } else if (message is PublicMessageFromCivilian) {
          outbox.AddRange(_banGenerator.Generate(contextualized));
          if (!outbox.Any()) { // Civilian hasn't been punished
            outbox.AddRange(_commandGenerator.Generate(contextualized));
          }
        }
      }
      return outbox;
    }

  }
}
