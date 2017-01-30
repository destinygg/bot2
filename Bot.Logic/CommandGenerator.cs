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

    public IReadOnlyList<ISendable> Generate(IContextualized<IUser, ITransmittable> contextualized) {
      var outbox = new List<ISendable>();
      var message = contextualized.Latest as IReceivedMessage<IUser>;
      if (message != null) {
        if (message.StartsWith("!time")) {
          outbox.Add(new SendablePublicMessage($"{_timeService.DestinyNow.ToShortTimeString()} Central Steven Time"));
        }
      }
      return outbox;
    }
  }
}
