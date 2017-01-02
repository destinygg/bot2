using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Logic {
  public class ModCommands : IModCommands {
    private readonly ILogger _logger;

    public ModCommands(ILogger logger) {
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
      return new PublicMessage("This is a debug command; output appears in log.");
    }

    public ISendable Sing() => new PublicMessage("/me sings a song");

  }
}
