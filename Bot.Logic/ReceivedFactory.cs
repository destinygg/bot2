﻿using System;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic {
  public class ReceivedFactory : IReceivedFactory {
    private readonly ITimeService _timeService;
    private readonly IModCommandParser _modCommandParser;
    private readonly IModCommandRegex _modCommandRegex;
    private readonly ILogger _logger;

    public ReceivedFactory(ITimeService timeService, IModCommandParser modCommandParser, IModCommandRegex modCommandRegex, ILogger logger) {
      _timeService = timeService;
      _modCommandParser = modCommandParser;
      _modCommandRegex = modCommandRegex;
      _logger = logger;
    }

    public PublicMessageFromMod ModPublicReceivedMessage(string text) => new PublicMessageFromMod(text, _timeService);
    public PublicMessageFromMod ModPublicReceivedMessage(string text, DateTime timestamp) => new PublicMessageFromMod(text, timestamp);

    public PublicMessageFromCivilian PublicReceivedMessage(string text) => new PublicMessageFromCivilian(text, _timeService);
    public PublicMessageFromCivilian PublicReceivedMessage(string text, DateTime timestamp) => new PublicMessageFromCivilian(text, timestamp);

    public ReceivedPardon ReceivedPardon(Moderator sender, Civilian target) => new ReceivedPardon(sender, target, _timeService);

    public ParsedNuke ParsedNuke(IReceived<Moderator, IMessage> message) => new ParsedNuke(message, _timeService, _modCommandRegex, _modCommandParser, _logger);
    public ParsedNuke ParsedNuke(string command) => new ParsedNuke(ModPublicReceivedMessage(command), _timeService, _modCommandRegex, _modCommandParser, _logger);
  }
}
