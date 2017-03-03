using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class CommandFactory : BaseSendableFactory<IUser, IMessage> {
    private readonly ITimeService _timeService;

    public CommandFactory(ITimeService timeService) {
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

    public override IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError("An error occured in the command factory.").Wrap().ToList();
  }
}
