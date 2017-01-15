using System;
using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Logic {
  public class CommandGenerator : ICommandGenerator {
    private readonly ITimeService _timeService;

    public CommandGenerator(ITimeService timeService) {
      _timeService = timeService;
    }

    public IReadOnlyList<ISendable> Generate(IContextualized contextualized) {
      var outbox = new List<ISendable>();
      var message = contextualized.First as ReceivedMessage;
      if (message != null) {
        if (message.StartsWith("!time")) {
          outbox.Add(new SendableMessage(_timeService.UtcNow.ToShortTimeString()));
        }
      }
      return outbox;
    }
  }
}
