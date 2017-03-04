using System;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic {
  public class ReceivedFactory : IReceivedFactory {
    private readonly ITimeService _timeService;
    private readonly IModCommandParser _modCommandParser;
    private readonly IModCommandRegex _modCommandRegex;
    private readonly ILogger _logger;
    private readonly ISettings _settings;

    public ReceivedFactory(ITimeService timeService, IModCommandParser modCommandParser, IModCommandRegex modCommandRegex, ILogger logger, ISettings settings) {
      _timeService = timeService;
      _modCommandParser = modCommandParser;
      _modCommandRegex = modCommandRegex;
      _logger = logger;
      _settings = settings;
    }

    public PublicMessageFromMod ModPublicReceivedMessage(string text) => new PublicMessageFromMod(text, _timeService);
    public PublicMessageFromMod ModPublicReceivedMessage(string text, DateTime timestamp) => new PublicMessageFromMod(text, timestamp);

    public PublicMessageFromCivilian PublicReceivedMessage(string text) => new PublicMessageFromCivilian(text, _timeService);
    public PublicMessageFromCivilian PublicReceivedMessage(string text, DateTime timestamp) => new PublicMessageFromCivilian(text, timestamp);

    public ReceivedPardon ReceivedPardon(Moderator sender, Civilian target) => new ReceivedPardon(sender, target, _timeService);

    public ParsedNuke ParsedNuke(IReceived<Moderator, IMessage> message) => new ParsedNuke(message, _timeService, _modCommandRegex, _modCommandParser, _logger, _settings);
  }
}
