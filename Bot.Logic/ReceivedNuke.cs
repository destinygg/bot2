﻿using System;
using System.Text.RegularExpressions;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class ReceivedNuke : IReceivedNuke {
    private readonly ITimeService _timeService;

    public ReceivedNuke(IReceivedMessage<Moderator> message, ITimeService timeService, IModCommandRegex modCommandRegex, IModCommandParser parser, ILogger logger) {
      _timeService = timeService;
      Timestamp = message.Timestamp;
      Sender = message.Sender;

      if (message.IsMatch(modCommandRegex.Nuke)) {
        var phraseDuration = parser.Nuke(message.Text);
        _matchesNukedTerm = _StringNuke(phraseDuration.Item1);
        Duration = phraseDuration.Item2;
      } else if (message.IsMatch(modCommandRegex.RegexNuke)) {
        var phraseDuration = parser.RegexNuke(message.Text);
        var regex = new Regex(phraseDuration.Item1, RegexOptions.IgnoreCase);
        _matchesNukedTerm = _RegexNuke(regex);
        Duration = phraseDuration.Item2;
      } else {
        logger.LogError($"ParsedNuke failed to parse:{message.Text}");
      }
    }

    private Predicate<string> _RegexNuke(Regex nukedRegex) => nukedRegex.IsMatch;

    private Predicate<string> _StringNuke(string nukedString) => possibleVictimText =>
      possibleVictimText.IgnoreCaseContains(nukedString) ||
      possibleVictimText.RemoveWhitespace().IgnoreCaseContains(nukedString) ||
      possibleVictimText.SimilarTo(nukedString) >= Settings.NukeMinimumStringSimilarity;

    public bool WillPunish(IReceivedMessage<Civilian> message) =>
      _matchesNukedTerm(message.Transmission.Text) &&
      _WithinRange(message) &&
      !_IsExpired(message);

    private readonly Predicate<string> _matchesNukedTerm;

    private bool _WithinRange(IReceivedMessage<Civilian> message) =>
      message.Timestamp.IsWithin(Timestamp, Settings.NukeBlastRadius);

    private bool _IsExpired(IReceivedMessage<IUser> message) {
      var punishmentTimestamp = message.Timestamp <= Timestamp ? Timestamp : message.Timestamp;
      var expirationDate = punishmentTimestamp + Duration;
      return expirationDate < _timeService.UtcNow;
    }

    public TimeSpan Duration { get; }
    public DateTime Timestamp { get; }
    public IUser Sender { get; }
  }
}
