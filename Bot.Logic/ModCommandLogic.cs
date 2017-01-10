using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;
using Bot.Tools;

namespace Bot.Logic {
  public class ModCommandLogic : IModCommandLogic {
    private readonly ILogger _logger;

    public ModCommandLogic(ILogger logger) {
      _logger = logger;
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
      return new SendableMessage("This is a debug command; output appears in log.");
    }

    public ISendable Sing() => new SendableMessage("/me sings a song");

    public IReadOnlyList<ISendable> Nuke(IReadOnlyList<IReceived> context, string phrase, TimeSpan duration) => context
      .OfType<ReceivedMessage>()
      .Where(m => m.Timestamp.IsWithin(Settings.NukeBlastRadius))
      .Where(m => m.Text.Contains(phrase) || m.Text.SimilarTo(phrase) >= Settings.NukeMinimumStringSimilarity)
      .Select(m => new SendableMute(m.Sender, duration)).ToList();
  }
}
