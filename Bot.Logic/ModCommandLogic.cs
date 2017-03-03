using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic {
  public class ModCommandLogic : IModCommandLogic {
    private readonly ILogger _logger;
    private readonly IErrorableFactory<IParsedNuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> _nukeMuteFactory;
    private readonly IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<Pardon>>> _aegisPardonFactory;

    public ModCommandLogic(
      IErrorableFactory<IParsedNuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> nukeMuteFactory,
      IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<Pardon>>> aegisPardonFactory,
      ILogger logger) {
      _logger = logger;
      _nukeMuteFactory = nukeMuteFactory;
      _aegisPardonFactory = aegisPardonFactory;
    }

    public ISendable<PublicMessage> Long(IReadOnlyList<IReceived<IUser, ITransmittable>> context) {
      _logger.LogInformation($"Long running process beginning, context length: {context.Count()}");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation("1");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation("2");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation("3");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation($"Long running process ending, context length: {context.Count()}");
      return new SendablePublicMessage("This is a debug command; output appears in log.");
    }

    public ISendable<PublicMessage> Sing() => new SendablePublicMessage("/me sings a song");

    public IReadOnlyList<ISendable<ITransmittable>> Nuke(IReadOnlyList<IReceived<IUser, ITransmittable>> context, IParsedNuke nuke) => _nukeMuteFactory.Create(nuke, context);

    public IReadOnlyList<ISendable<Pardon>> Aegis(IReadOnlyList<IReceived<IUser, ITransmittable>> context) => _aegisPardonFactory.Create(context);
  }
}
