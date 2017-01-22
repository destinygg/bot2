using System;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public class ReceivedFactory : IReceivedFactory {
    private readonly ITimeService _timeService;
    private readonly IModCommandParser _modCommandParser;

    public ReceivedFactory(ITimeService timeService, IModCommandParser modCommandParser) {
      _timeService = timeService;
      _modCommandParser = modCommandParser;
    }

    public PublicMessageFromMod ModPublicReceivedMessage(string text) => new PublicMessageFromMod(text, _timeService);
    public PublicMessageFromMod ModPublicReceivedMessage(string text, DateTime timestamp) => new PublicMessageFromMod(text, timestamp);

    public PublicReceivedMessage PublicReceivedMessage(string text) => new PublicReceivedMessage(text, _timeService);
    public PublicReceivedMessage PublicReceivedMessage(string text, DateTime timestamp) => new PublicReceivedMessage(text, timestamp);

    public ReceivedPardon ReceivedPardon(IUser sender, IUser target) => new ReceivedPardon(sender, target, _timeService);

    public ReceivedRegexNuke ReceivedRegexNuke(ReceivedMessage message) => new ReceivedRegexNuke(message, _modCommandParser);
    public ReceivedStringNuke ReceivedStringNuke(ReceivedMessage message) => new ReceivedStringNuke(message, _modCommandParser);

    public ReceivedRegexNuke ReceivedRegexNuke(string command) => new ReceivedRegexNuke(ModPublicReceivedMessage(command), _modCommandParser);
    public ReceivedStringNuke ReceivedStringNuke(string command) => new ReceivedStringNuke(ModPublicReceivedMessage(command), _modCommandParser);
  }
}
