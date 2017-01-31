using System.Collections.Generic;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class CommandGenerator : ICommandGenerator {
    private readonly ITimeService _timeService;

    public CommandGenerator(ITimeService timeService) {
      _timeService = timeService;
    }

    public IReadOnlyList<ISendable> Generate(ISnapshot<IUser, ITransmittable> snapshot) {
      var outbox = new List<ISendable>();
      var message = snapshot.Latest as IReceivedMessage<IUser>;
      if (message != null) {
        if (message.StartsWith("!time")) {
          outbox.Add(new SendablePublicMessage($"{_timeService.DestinyNow.ToShortTimeString()} Central Steven Time"));
        }
      }
      return outbox;
    }
  }
}
