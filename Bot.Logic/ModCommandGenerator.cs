using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Logic {

  public class ModCommandGenerator : IModCommandGenerator {
    private readonly ILogger _logger;

    public ModCommandGenerator(ILogger logger) {
      _logger = logger;
    }

    public IReadOnlyList<ISendable> Generate(IContextualized contextualized) {
      var outbox = new List<ISendable>();
      var context = contextualized.Context;
      var message = contextualized.First as IPublicMessageReceived;
      if (message != null) {
        if (!message.FromMod) return outbox;
        if (message.StartsWith("!sing"))
          outbox.Add(new PublicMessage("/me sings a song"));
        if (message.StartsWith("!long")) {
          _logger.LogInformation("long begin" + context.Count());
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
          _logger.LogInformation("long over" + context.Count());
        }
      }
      return outbox;
    }
  }
}
