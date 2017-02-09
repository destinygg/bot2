using System.Collections.Generic;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class CommandGenerator : BaseSendablesFactory<IUser, IMessage> {
    private readonly ITimeService _timeService;

    public CommandGenerator(ITimeService timeService) {
      _timeService = timeService;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<IUser, IMessage> snapshot) {
      var outbox = new List<ISendable<PublicMessage>>();
      var message = snapshot.Latest;
      if (message.StartsWith("!time")) {
        outbox.Add(new SendablePublicMessage($"{_timeService.DestinyNow.ToShortTimeString()} Central Steven Time"));
      }
      return outbox;
    }
  }
}
