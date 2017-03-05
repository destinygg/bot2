using System;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class ReceivedFactory : IReceivedFactory {
    private readonly ITimeService _timeService;
    private readonly IFactory<IReceived<Moderator, IMessage>, ParsedNuke> _nukeFactory;

    public ReceivedFactory(ITimeService timeService, IFactory<IReceived<Moderator, IMessage>, ParsedNuke> nukeFactory) {
      _timeService = timeService;
      _nukeFactory = nukeFactory;
    }

    public PublicMessageFromMod ModPublicReceivedMessage(string text) => new PublicMessageFromMod(text, _timeService);
    public PublicMessageFromMod ModPublicReceivedMessage(string text, DateTime timestamp) => new PublicMessageFromMod(text, timestamp);

    public PublicMessageFromCivilian PublicReceivedMessage(string text) => new PublicMessageFromCivilian(text, _timeService);
    public PublicMessageFromCivilian PublicReceivedMessage(string text, DateTime timestamp) => new PublicMessageFromCivilian(text, timestamp);

    public ReceivedPardon ReceivedPardon(Moderator sender, Civilian target) => new ReceivedPardon(sender, target, _timeService);

    public ParsedNuke ParsedNuke(IReceived<Moderator, IMessage> message) => _nukeFactory.Create(message);
  }
}
