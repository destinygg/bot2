using System;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Models.Snapshot;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class ReceivedFactory {
    private readonly ITimeService _timeService;

    public ReceivedFactory(ITimeService timeService) {
      _timeService = timeService;
    }

    public PublicMessageFromMod ModPublicReceivedMessage(string text) => new PublicMessageFromMod(text, _timeService);
    public PublicMessageFromMod ModPublicReceivedMessage(string text, DateTime timestamp) => new PublicMessageFromMod(text, timestamp);

    public PublicMessageFromCivilian PublicReceivedMessage(string text) => new PublicMessageFromCivilian(text, _timeService);
    public PublicMessageFromCivilian PublicReceivedMessage(string text, DateTime timestamp) => new PublicMessageFromCivilian(text, timestamp);

    public ReceivedPardon ReceivedPardon(Moderator sender, Civilian target) => new ReceivedPardon(sender, target, _timeService);
    public PublicMessageFromCivilianSnapshot PublicReceivedSnapshot(string text) =>
      new PublicMessageFromCivilianSnapshot(PublicReceivedMessage(text), Enumerable.Empty<IReceived<IUser, ITransmittable>>().ToList());
    public PublicMessageFromCivilianSnapshot PublicReceivedSnapshot(string text, DateTime timestamp) =>
      new PublicMessageFromCivilianSnapshot(PublicReceivedMessage(text, timestamp), Enumerable.Empty<IReceived<IUser, ITransmittable>>().ToList());
  }
}
