using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Logic {
  public class ModCommandLogic : IModCommandLogic {
    private readonly ILogger _logger;
    private readonly INukeLogic _nukeLogic;

    public ModCommandLogic(ILogger logger, INukeLogic nukeLogic) {
      _logger = logger;
      _nukeLogic = nukeLogic;
    }

    public ISendable Long(IReadOnlyList<IReceived> context) {
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

    public ISendable Sing() => new SendablePublicMessage("/me sings a song");

    public IReadOnlyList<ISendable> Nuke(IReadOnlyList<IReceived> context, IReceivedNuke nuke)
      => _nukeLogic.Nuke(nuke, context);


    public IReadOnlyList<ISendable> Aegis(IReadOnlyList<IReceived> context)
      => _nukeLogic.Aegis(context);
  }
}
