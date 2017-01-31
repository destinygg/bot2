using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

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

    public IReadOnlyList<ISendable> Generate(ISnapshot<IUser, ITransmittable> snapshot) {
      var outbox = new List<ISendable>();
      var message = snapshot.Latest as IReceived<IUser, IMessage>;
      if (message != null) {
        _logger.LogVerbose(message.Transmission.Text);
        if (message.IsFromMod()) {
          outbox.AddRange(_modCommandGenerator.Generate(snapshot));
          outbox.AddRange(_commandGenerator.Generate(snapshot));
        } else if (message is PublicMessageFromCivilian) {
          outbox.AddRange(_banGenerator.Generate(snapshot));
          if (!outbox.Any()) { // Civilian hasn't been punished
            outbox.AddRange(_commandGenerator.Generate(snapshot));
          }
        }
      }
      return outbox;
    }

  }
}
